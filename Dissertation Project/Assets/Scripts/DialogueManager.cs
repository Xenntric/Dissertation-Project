using Godot;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

public class DialogueManager : Node
{
	
	//public Godot.Collections.Dictionary<T> Characters;
	/* [Export]
	public List<StaticBody2D> Characters; */
	// Declare member variables here. Examples:
	//public System.IO.File text_file;

	/* public string TextFile;
    public List<string> TextLines;
    public List<Node> Characters;
    private int lines;
    private int chars;
    private string LineToRead;*/

    private Godot.Collections.Array<Node> NPCs;
    private List<Tuple<Node,string>> tFiles;
    private DialogueLoader DL;
    private List<Node> NPCstack;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        NPCs = new Godot.Collections.Array<Node>(GetNode("../NPCs").GetChildren());
        tFiles = new List<Tuple<Node, string>>(DL.LoadTextFiles(NPCs));
        NPCstack = new List<Node>();

        /* for (int i = 0; i < NPCs.Count; i++)
        {
            GD.Print("Script " + i + "\n" + tFiles[i].Item1.Name + "\n"
            + tFiles[i].Item2);
        } */

        

        //chars = GetNode("../../NPCs").GetChildCount();
        //GD.Print(chars);
        
        //TextLines = new List<string>();
		//Characters = new Godot.Collections.Dictionary<int>();
		//text_string = System.IO.File.ReadAllText($"Assets/Dialogue/TestDialogue");
		//GD.Print(text_string);

		/* TextFile = @"Assets/Dialogue/TestDialogue";
		using (StreamReader ReaderObject = new StreamReader(TextFile))
		{
			string line;
			while((line = ReaderObject.ReadLine()) != null)
            {
                lines++;
                GD.Print(line);
                TextLines.Add(line);
            }
            GD.Print(lines);
		}*/

		/* int counter = 0;
		foreach (var line in text_string)
		{
			GD.Print(line);
			counter++;
			GD.Print("end of line 1");
		} */
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
                    GD.Print(tFiles[i].Item2);
                }
            }
        }
    }

    public void AddNPC(Node NPC)
    {
        NPCstack.Add(NPC);
    }
    public void PopNPC()
    {
        NPCstack.RemoveAt(0);
    }
}
