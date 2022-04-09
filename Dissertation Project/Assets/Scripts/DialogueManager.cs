using Godot;
using System;
using Godot.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;


public class DialogueManager : Node
{
    private DialogueLoader DL;
    private List<Node> NPCstack;
    [Export] private List<Texture> Emotes;
    private Sprite CurrentEmote;
    private List<NPCs> NPCList;
    private RichTextLabel CurrentText;

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
            NPCList[i].SetCharacter(GetNode("../NPCs").GetChild(i));
            NPCList[i].SetTextFile(DL.LoadFile(NPCList[i].GetCharacter()));
            NPCList[i].SetLines(DL.LoadLines(NPCList[i].GetTextFile()));
        }
	}
    public void ReadLine()
    {
        if(NPCstack != null)
        {
            for (int i = 0; i < NPCList.Count; i++)
            {
                if(NPCstack.Contains(NPCList[i].GetCharacter()))
                {
                    GD.Print("NPC " + NPCList[i].GetCharacter().Name);
                    //GD.Print("Test: " + NPCList[i].GetCharacter().Name + "\n" + NPCList[i].GetLines()[0]);

                    var DialogueToEnact = ParseLine(NPCList[i], NPCList[i].GetTextFile());

                    //if an emote sprite is already present, get rid of it
                    if (CurrentEmote.Texture != null)
                    {
                        CurrentEmote.Texture = null;
                    }

                    if (DialogueToEnact.Item2 != null)
                    {
                        //Line has both dialogue and an emote
                        MatchEmoteToImage(NPCList[i].GetCharacter(), DialogueToEnact.Item2);
                    }

                    //Print dialogue
                    DisplayText(i, DialogueToEnact);
                    
                    //Increase TimesTalkedTo counter
                    NPCList[i].SetTimesTalkedTo(NPCList[i].GetTimesTalkedTo() + 1);
                }
            }
        }
    }
    
    public Tuple<string,string> ParseLine(NPCs character, string text)
    {
        GD.Print("Times Talked To: " + character.GetTimesTalkedTo());
        var line = character.GetLines()[character.GetTimesTalkedTo()];
        var dialogue = Regex.Match(line, @"(^[^|]*)");

        //Does string even have Emote qualifier?
        if(Regex.Match(line, @"[|]+").Success)
        {
            //Capture string after | and before Endline
            var emoteReg = Regex.Match(line, @"\|(.*?)\n");
            string emote = PopFirstChar(emoteReg);
            emote = String.Concat(emote.Where(c => !Char.IsWhiteSpace(c)));
            return new Tuple<string, string>(dialogue.ToString(),emote);
        }
        else
        {
            return new Tuple<string, string>(dialogue.ToString(),null);
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

    private void DisplayText(int i, Tuple<string, string> DialogueToEnact)
    {
        string NPCsPath = NPCList[i].GetCharacter().GetPath();
        var TextPoint = GetNode(new NodePath(NPCsPath + "/" + "Targets")).GetChild(1) as Node2D;

        CurrentText.RectSize = new Vector2(CurrentText.GetFont("mono_font").GetStringSize(DialogueToEnact.Item1) * 2);
        var centre = new Vector2(CurrentText.RectSize.x / 2, 0);
        CurrentText.BbcodeText = ("[center]" + DialogueToEnact.Item1 + "[/center]");

        CurrentText.SetGlobalPosition(new Vector2(TextPoint.Position) - centre);
        CurrentText.ShowOnTop = true;
        CurrentText.SetIndexed("position:z", 100);
    }

    private static float GetStringWidth(string text, Font font)
    {
        float stringPixelWidth = new float();
        foreach (char letter in text)
        {
            var letterPixelWidth = font.GetCharSize(letter);
            stringPixelWidth = stringPixelWidth + letterPixelWidth.x;
        }
        GD.Print("width of string: " + stringPixelWidth);
        return stringPixelWidth;
    }

    private static string PopFirstChar(Match emoteReg)
    {
        List<char> StripChar = new List<char>(emoteReg.ToString().ToCharArray());
        StripChar.RemoveAt(0);
        var emote = new string(StripChar.ToArray());
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