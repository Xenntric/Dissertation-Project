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
    private DialogueLoader DL;
    private List<Node> NPCstack;
    private Sprite CurrentEmote;
    private List<NPCs> NPCList;
    private RichTextLabel CurrentText;
    private RegexCheck RC;

    private  bool readLine = false;

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

                    enumerator = SetEnumerator(NPCList[i], ReadLine, ref linetosay);
                    ActOnEnumerator(i, enumerator, character, linetosay);

                    GD.Print("NPC " + NPCList[i].Character.Name);
                    GD.Print("Times Talked To: " + NPCList[i].TimesTalkedTo);
                }
            }
        }
    }

    private RegexCheck.Parse SetEnumerator(NPCs npc, string ReadLine, ref Tuple<string, string> linetosay)
    {
        RegexCheck.Parse enumerator;
       
        if (RC.END.IsMatch(ReadLine))
        {
            enumerator = RegexCheck.Parse.End;
        }
        else if(RC.PLAY.IsMatch(ReadLine))
        {
            foreach (Node child in npc.Character.GetChildren())
            {
                if(child is AnimationPlayer)
                {
                    npc.AnimationPlayer = GetNode<AnimationPlayer>(child.GetPath());
                }
            }

            //Get Everything between the parentheses 
            var SetLine = Regex.Match(ReadLine, @"\((.*?)\)").ToString();
            GD.Print(SetLine);
            SetLine = DL.PopFirstLastandWhitespaces(SetLine);
            
            GD.Print(SetLine);

            npc.AnimationPlayer.Play(SetLine);
            enumerator = RegexCheck.Parse.PlayAnimation;
        }
        else if (RC.IF.IsMatch(ReadLine))
        {
            var SetLine = DL.PopFirstLastandWhitespaces(Regex.Match(ReadLine, @"\((.*?)\)").ToString());
            if(Regex.Match(SetLine, @"[=]").Success && Regex.Match(SetLine, @"[0-9]").Success)
            {
                GD.Print("Integer IF");
            }
            else
            {
                GD.Print("Boolean IF");
                if(SetLine.Substr(0,1) != "!")
                {
                    //if True
                    foreach (var key in npc.Bools.Keys)
                    {
                        if(key == SetLine && npc.Bools[key])
                        {
                            readLine = true;
                        }
                        else
                        {
                            readLine = false;
                        }
                    }
                }
                else if(SetLine.Substr(0,1) == "!")
                {
                    SetLine = PopChar(SetLine, true);
                    //if false;
                    foreach (var key in npc.Bools.Keys)
                    {

                        if(key == SetLine && !npc.Bools[key])
                        {
                            readLine = true;
                        }
                        else
                        {
                            readLine = false;
                        }
                    }
                }
            } 

            enumerator = RegexCheck.Parse.If;
        }

        else if (RC.SET.IsMatch(ReadLine))
        {
            GD.Print("Found Set");
            SetInTextVariable(ReadLine);
            enumerator = RegexCheck.Parse.Set;
        }

        else
        {
            if(ReadLine.Substr(0,1) == "*" && !readLine)
            {
                    GD.Print("skipping");
                    enumerator = RegexCheck.Parse.SkipLine;
                    return enumerator;
            }
            //if an emote sprite is already present, get rid of it
            if (CurrentEmote.Texture != null)
            {
                CurrentEmote.Texture = null;
            }
            if (RC.HasEMOTE(ReadLine))
            {
                linetosay = new Tuple<string, string>(
                    RC.DIALOGUE.Match(ReadLine).ToString(),
                    PopChar(RC.EMOTE.Match(ReadLine).ToString(),true));

                enumerator = RegexCheck.Parse.DialogueEmote;
                GD.Print("Dialogue to read: " + linetosay.Item1);
                GD.Print("emote to read: " + linetosay.Item2);
            }
            else
            {
                linetosay = new Tuple<string, string>(
                    RC.DIALOGUE.Match(ReadLine).ToString(),null);
                    
                GD.Print("No Emote Found");
                enumerator = RegexCheck.Parse.Dialogue;
            }
        }

        return enumerator;
    }

    private void SetInTextVariable(string ReadLine)
    {
        //Get Everything between the parentheses 
        var SetLine = Regex.Match(ReadLine, @"\((.*?)\)").ToString();
        //Get String before '='
        var SetVar = DL.PopFirstLastandWhitespaces(Regex.Match(SetLine, (@"\((.*?)\=")).ToString());
        //Get Character mentioned before the '.'
        var SetCharacter = DL.PopFirstLastandWhitespaces(Regex.Match(SetLine, (@"\((.*?)\.")).ToString());
        //Get Variable after '.'
        var SetType = DL.PopFirstLastandWhitespaces(Regex.Match(SetLine, (@"\.(.*?)\=")).ToString());
        //Get Value after '='
        var SetValue = DL.PopFirstLastandWhitespaces(Regex.Match(SetLine, (@"\=(.*?)\)")).ToString());

        GD.Print("Var = " + SetVar);
        GD.Print("Character = " + SetCharacter);
        GD.Print("Type = " + SetType);
        GD.Print("Value = " + SetValue);


        foreach (var NPC in NPCList)
        {
            if (NPC.Character.Name == SetCharacter)
            {
                GD.Print("Found Character");

                foreach (var key in NPC.Bools.Keys)
                {
                    GD.Print("Finding Bools");
                    if (key == SetType)
                    {
                        GD.Print("Found Key");
                        GD.Print("Character: " + NPC.Character.Name + " with key: " + key + " with value: " + NPC.Bools[key]);
                        NPC.Bools[key] = System.Boolean.Parse(SetValue);
                        GD.Print("Character: " + NPC.Character.Name + " with key: " + key + " with value: " + NPC.Bools[key]);
                        GD.Print("Set Key");
                        break;
                    }

                    GD.Print("Checking next Bool");
                }

                GD.Print("Moving on to ints...");

                foreach (var key in NPC.Ints.Keys)
                {
                    GD.Print("Finding Ints");
                    if (key == SetType)
                    {
                        GD.Print("Found Key");
                        NPC.Ints[key] = System.Int32.Parse(SetValue);
                        break;
                    }
                }

            }
        }
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
                    GD.Print("Ending");
                    DisplayText(character, null);
                    GD.Print("Ended");

                    break;
                }
            case RegexCheck.Parse.If:
                {
                    
                    //Because this line is a function, I don't want it to interrupt the flow of the
                    //Dialogue, so I am forcing the next line to get read
                    NPCList[i].TimesTalkedTo = (NPCList[i].TimesTalkedTo + 1);

                    ReadLine();
                    break;
                }
            case RegexCheck.Parse.Set:
                {
                    GD.Print("Incrementing Talked to");
                    
                    //Because this line is a function, I don't want it to interrupt the flow of the
                    //Dialogue, so I am forcing the next line to get read
                    NPCList[i].TimesTalkedTo = (NPCList[i].TimesTalkedTo + 1);

                    ReadLine();
                    break;
                }
            case RegexCheck.Parse.SkipLine:
                {
                    NPCList[i].TimesTalkedTo = (NPCList[i].TimesTalkedTo + 1);
                    ReadLine();
                    break;                
                }
            case RegexCheck.Parse.PlayAnimation:
                {
                    NPCList[i].TimesTalkedTo = (NPCList[i].TimesTalkedTo + 1);
                    ReadLine();
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
        if(DialogueToEnact != null && DialogueToEnact.Substr(0,1) == "*")
        {
            DialogueToEnact = PopChar(DialogueToEnact, false);
        }
        CurrentText.BbcodeText = ("[center]" + DialogueToEnact + "[/center]");

        CurrentText.SetGlobalPosition(new Vector2(TextPoint.Position) - centre);
        CurrentText.ShowOnTop = true;
        //CurrentText.SetIndexed("position:z", 100);
    }

    private static string PopChar(string emoteReg, bool removeWhiteSpaces)
    {
        GD.Print("To strip: " + emoteReg.ToString());

        List<char> StripChar = new List<char>(emoteReg.ToCharArray());
        StripChar.RemoveAt(0);
        var emote = new string(StripChar.ToArray());
        if(removeWhiteSpaces)
        {
            emote = String.Concat(emote.Where(c => !Char.IsWhiteSpace(c)));
        }
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