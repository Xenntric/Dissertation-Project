using Godot;
using System;
using System.IO;

public struct DialogueLoader
{
    private Godot.Collections.Array<Node> LoadCharacters;
    System.Collections.Generic.List<string> TextFiles;
    
    public System.Collections.Generic.List<string> LoadTextFiles(Godot.Collections.Array<Node> Characters)
    {
        TextFiles = new System.Collections.Generic.List<string>();
        this.LoadCharacters = Characters;

        for (int i = 0; i < this.LoadCharacters.Count; i++)
        {
            var address = System.IO.Path.Combine(@"Assets/Dialogue/", this.LoadCharacters[i].Name.ToLower());
            TextFiles.Add(System.IO.File.ReadAllText(address));
            GD.Print("text file " + i + ":\n" + TextFiles[i]);
        }

        return TextFiles;
    }
}
