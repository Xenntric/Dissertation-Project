using Godot;
using System;
using Godot.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;


public class DialogueManager : Node
{
    private List<Tuple<Node,string>> tFiles;
    private DialogueLoader DL;
    private List<Node> NPCstack;
    [Export] public List<Texture> Emotes;
    public List<NPCs> NPCList;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        NPCstack = new List<Node>();
        NPCList = new List<NPCs>();

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

                    if (DialogueToEnact.Item2 == null)
                    {
                        //Line only has dialogue
                        GD.Print(DialogueToEnact.Item1);
                    }
                    else
                    {
                        //Line has both dialogue and an emote
                        GD.Print(DialogueToEnact.Item1);
                        GD.Print(DialogueToEnact.Item2);
                        MatchEmoteToImage(NPCList[i].GetCharacter(), DialogueToEnact.Item2);
                    }
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


        if(Regex.Match(line, @"[|]+").Success)
        {
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
            
            //GD.Print(image.ResourceName.ToString());
            //GD.Print(emote.ToString());
            if (emote == emoteFilename.ToString())
            {
                string NPCsPath = "../NPCs/" + NPC.Name;
                var NPCproperties = GetNode<StaticBody2D>(new NodePath(NPCsPath));

                //Targets the EmotePoint node which needs to be the first Child of 'Targets'
                var EmotePoint = GetNode(new NodePath(NPCsPath + "/" + "Targets")).GetChild(0) as Node2D;

                var img = new Sprite();
                img.Texture = image;
                img.Scale = new Vector2(NPCproperties.Scale);
                img.Position = new Vector2(EmotePoint.Position);
                img.ZIndex = NPCproperties.ZIndex + 1;
                
                //GD.Print(NPC.Name);
                AddChild(img);
            }
            else
            {
                //GD.Print("damn");
            }
        }
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
