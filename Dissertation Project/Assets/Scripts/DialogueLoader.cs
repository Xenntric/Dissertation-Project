using Godot;
using System;
using System.IO;
using System.Collections.Generic;

public struct DialogueLoader
{
    private Godot.Collections.Array<Node> LoadCharacters;
    private System.Collections.Generic.List<string> TextFiles;
    private System.Collections.Generic.List<Tuple<Node,string>> PairedScripts;
    
    public System.Collections.Generic.List<Tuple<Node,string>> LoadTextFiles(Godot.Collections.Array<Node> Characters)
    {
        PairedScripts = new List<Tuple<Node,string>>();
        TextFiles = new List<string>();
        LoadCharacters = Characters;

        for (int i = 0; i < LoadCharacters.Count; i++)
        {
            var address = System.IO.Path.Combine(@"Assets/Dialogue/", LoadCharacters[i].Name.ToLower());
            TextFiles.Add(System.IO.File.ReadAllText(address));
            PairedScripts.Add(new Tuple<Node, string>(LoadCharacters[i],TextFiles[i]));
        }
        return PairedScripts;
    }
}
