using Godot;
using System;
using System.IO;
using System.Text.RegularExpressions;

using System.Collections.Generic;

public struct DialogueLoader
{
    private System.Collections.Generic.List<string> TextFiles;
    private System.Collections.Generic.List<Tuple<Node,string>> PairedScripts;
    
    public string LoadFile(Node Character)
    {
        var address = System.IO.Path.Combine(@"Assets/Dialogue/",Character.Name.ToLower());
        var file = System.IO.File.ReadAllText(address);
        return file.ToString();
    }

    public List<string> LoadLines(string file)
    {
        var lineList = new List<string>();
        //@"(/\r?\n/g)"
        foreach (Match match in Regex.Matches(file, @"(.*?)\n"))
        {
            //@"(.*?)\n"
            var line = match.ToString();
            lineList.Add(line);
        }
        return lineList;
    }
}
