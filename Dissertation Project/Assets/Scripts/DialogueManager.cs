using Godot;
using System;
using Godot.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;


public class DialogueManager : Node
{
    [Export] private List<Texture> Emotes;
    [Export] private List<bool> BoolFlag;
    [Export] private List<int> IntFlag;
    private DialogueLoader DL;
    private List<Node> NPCstack;
    private Sprite CurrentEmote;
    private List<NPCs> NPCList;
    private RichTextLabel CurrentText;

    private RegexCheck RegCheck;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        NPCstack = new List<Node>();
        NPCList = new List<NPCs>();

        AddChild(CurrentEmote = new Sprite());
        AddChild(CurrentText = new RichTextLabel());
        

        CurrentText.BbcodeEnabled = true;

        for (int i = 0; i < GetNode("../NPCs").GetChildren().Count; i++)
        {
            NPCList.Add(new NPCs());
            NPCList[i].Character = (GetNode("../NPCs").GetChild(i));
            NPCList[i].TextFile = (DL.LoadFile(NPCList[i].Character));


            //If there's no file, don't bother trying to find the lines.
            if(NPCList[i].TextFile != null )
            {
                NPCList[i].Lines = (DL.LoadLines(NPCList[i].TextFile));
                NPCList[i].Bools = (DL.LoadBools(NPCList[i].Lines));
                NPCList[i].Ints  = (DL.LoadInts(NPCList[i].Lines));

                foreach (var Bool in NPCList[i].Bools)
                {
                    GD.Print("Bools @ " + NPCList[i].Character.Name + ": " + Bool.Key.ToString());
                    NPCList[i].TimesTalkedTo++;
                }
                foreach (var Int in NPCList[i].Ints)
                {
                    GD.Print("Ints @ " + NPCList[i].Character.Name + ": " + Int.Key.ToString() + " = " + Int.Value.ToString());
                    NPCList[i].TimesTalkedTo++;

                }
            }
        }
	}
    public void ReadLine()
    {
        //If the Player is near an NPC
        if(NPCstack != null)
        {  
            //For(amount of NPCs in Scene)
            for (int i = 0; i < NPCList.Count; i++)
            {
                //First Match of Nearby NPC
                if(NPCstack.Contains(NPCList[i].Character))
                {
                    var enumerator = new RegexCheck.Parse();

                    /***CATCH FOR NOT FINDING FILE ***/
                    if (NPCList[i].TextFile == null)
                    {
                        GD.Print("No File for " + NPCList[i].Character.Name + " found");
                        return;
                    }

                    var ReadLine = NPCList[i].Lines[NPCList[i].TimesTalkedTo];
                    var character = NPCList[i].Character;
                    Tuple<string, string> linetosay = new Tuple<string, string>(null, null);

                    enumerator = SetEnumerator(ReadLine, ref linetosay);
                    ActOnEnumerator(i, enumerator, character, linetosay);

                    GD.Print("NPC " + NPCList[i].Character.Name);
                    GD.Print("Times Talked To: " + NPCList[i].TimesTalkedTo);
                }
            }
        }
    }

    private RegexCheck.Parse SetEnumerator(string ReadLine, ref Tuple<string, string> linetosay)
    {
        RegexCheck.Parse enumerator;
        if (RegCheck.END.IsMatch(ReadLine))
        {
            enumerator = RegexCheck.Parse.End;
        }
        else
        {
            //if an emote sprite is already present, get rid of it
            if (CurrentEmote.Texture != null)
            {
                CurrentEmote.Texture = null;
            }

            if (RegCheck.HasEMOTE(ReadLine))
            {
                linetosay = new Tuple<string, string>(
                    RegCheck.DIALOGUE.Match(ReadLine).ToString(),
                    PopChar(RegCheck.EMOTE.Match(ReadLine)));

                enumerator = RegexCheck.Parse.DialogueEmote;
                GD.Print("Dialogue to read: " + linetosay.Item1);
                GD.Print("emote to read: " + linetosay.Item2);
            }
            else
            {
                linetosay = new Tuple<string, string>(
                    RegCheck.DIALOGUE.Match(ReadLine).ToString(),null);
                    
                GD.Print("No Emote Found");
                enumerator = RegexCheck.Parse.Dialogue;
            }
        }

        return enumerator;
    }

    private void ActOnEnumerator(int i, RegexCheck.Parse enumerator, Node character, Tuple<string, string> linetosay)
    {
        switch (enumerator)
        {
            case RegexCheck.Parse.DialogueEmote:
                {
                    DisplayText(character, linetosay.Item1);
                    MatchEmoteToImage(character, linetosay.Item2);

                    NPCList[i].TimesTalkedTo = (NPCList[i].TimesTalkedTo + 1);
                    break;
                }
            case RegexCheck.Parse.Dialogue:
                {
                    DisplayText(character, linetosay.Item1);

                    NPCList[i].TimesTalkedTo = (NPCList[i].TimesTalkedTo + 1);
                    break;
                }
            case RegexCheck.Parse.End:
                {
                    DisplayText(character, null);
                    break;
                }

            default:
                break;
        }
    }

    private void MatchEmoteToImage(Node NPC, string emote)
    {
        emote = emote + ".PNG";
        foreach (var image in Emotes)
        {
            var emoteFilename = Regex.Match(image.ResourcePath.ToString(), @"[^\/]+$");
            
            string NPCsPath = NPC.GetPath();
            var NPCproperties = GetNode<StaticBody2D>(NPCsPath);
            if (emote == emoteFilename.ToString())
            {
                DisplayEmote(image, NPCsPath, NPCproperties);
            }
            else
            {
                //Emote is not a recognised emote
            }
        }
    }

    private void DisplayEmote(Texture image, string NPCsPath, StaticBody2D NPCproperties)
    {
        //Targets the EmotePoint node which needs to be the first Child of 'Targets'
        var EmotePoint = GetNode(new NodePath(NPCsPath + "/" + "Targets")).GetChild(0) as Node2D;

        //Set Emote to desired PNG
        CurrentEmote.Texture = image;
        CurrentEmote.Scale = new Vector2(NPCproperties.Scale);
        CurrentEmote.Position = new Vector2(EmotePoint.Position);
        CurrentEmote.ZIndex = NPCproperties.ZIndex + 1;
    }

    private void DisplayText(Node NPC, string DialogueToEnact)
    {
        string NPCsPath = NPC.GetPath();
        var TextPoint = GetNode(new NodePath(NPCsPath + "/" + "Targets")).GetChild(1) as Node2D;

        CurrentText.RectSize = new Vector2(CurrentText.GetFont("mono_font").GetStringSize(DialogueToEnact) * 2);
        var centre = new Vector2(CurrentText.RectSize.x / 2, 0);
        CurrentText.BbcodeText = ("[center]" + DialogueToEnact + "[/center]");

        CurrentText.SetGlobalPosition(new Vector2(TextPoint.Position) - centre);
        CurrentText.ShowOnTop = true;
        //CurrentText.SetIndexed("position:z", 100);
    }

    private static string PopChar(Match emoteReg)
    {
        GD.Print("To strip: " + emoteReg.ToString());

        List<char> StripChar = new List<char>(emoteReg.ToString().ToCharArray());
        StripChar.RemoveAt(0);
        var emote = new string(StripChar.ToArray());
        emote = String.Concat(emote.Where(c => !Char.IsWhiteSpace(c)));
        return emote;
    }

    public void AddNPC(Node NPC)
    {
        NPCstack.Add(NPC);
    }
    public void PopNPC()
    {
        NPCstack.RemoveAt(0);
    }

    

    /* EditorSelection editorSelection;
    public Godot.Collections.Array returnSelection()
    {
        return editorSelection.GetSelectedNodes();
    }  */
}