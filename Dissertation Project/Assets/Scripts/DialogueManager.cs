using Godot;
using System;
using Godot.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;


public class DialogueManager : Node
{
    private Godot.Collections.Array<Node> NPCs;
    private List<Tuple<Node,string>> tFiles;
    private DialogueLoader DL;
    private List<Node> NPCstack;
    [Export] public List<Texture> Emotes;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        NPCs = new Godot.Collections.Array<Node>(GetNode("../NPCs").GetChildren());
        tFiles = new List<Tuple<Node, string>>(DL.LoadTextFiles(NPCs));
        NPCstack = new List<Node>();
	}

    public void ReadLine()
    {
        if(NPCstack != null)
        {
            for (int i = 0; i < tFiles.Count; i++)
            {
                if(NPCstack.Contains(tFiles[i].Item1))
                {
                    GD.Print("NPC " + tFiles[i].Item1.Name);
                    var DialogueToEnact = ParseLine(tFiles[i].Item2);
                    
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
                        MatchEmoteToImage(tFiles[i].Item1, DialogueToEnact.Item2);
                    }
                }
            }
        }
    }

    public Tuple<string,string> ParseLine(string text)
    {

        var line = Regex.Match(text, @"(.*?)\n");
        var dialogue = Regex.Match(line.ToString(), @"(^[^|]*)");

        if(Regex.Match(line.ToString(), @"[|]+").Success)
        {
            var emoteReg = Regex.Match(line.ToString(), @"\|(.*?)\n");
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
            
            GD.Print(image.ResourceName.ToString());
            //GD.Print(emote.ToString());
            if (emote == emoteFilename.ToString())
            {
                GD.Print("ding");
                string NPCsPath = "../NPCs/" + NPC.Name;
                var NPCproperties = GetNode<StaticBody2D>(new NodePath(NPCsPath));

                //Targets the EmotePoint node which needs to be the first Child of 'Targets'
                var EmotePoint = GetNode(new NodePath(NPCsPath + "/" + "Targets")).GetChild(0) as Node2D;

                var img = new Sprite();
                img.Texture = image;
                img.Scale = new Vector2(NPCproperties.Scale);
                img.Position = new Vector2(EmotePoint.Position);
                img.ZIndex = NPCproperties.ZIndex + 1;
                
                GD.Print(NPC.Name);
                AddChild(img);
            }
            else
            {
                GD.Print("dong");
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
