using Godot;
using System;
using System.IO;
using System.Text.RegularExpressions;

using System.Collections.Generic;
using System.Linq;

public struct DialogueLoader
{
    private System.Collections.Generic.List<string> TextFiles;
    private System.Collections.Generic.List<Tuple<Node,string>> PairedScripts;

    private List<bool>BoolsToInject;
    private List<int>IntsToInject;
    
    public string LoadFile(Node Character)
    {
        var address = System.IO.Path.Combine(@"Assets/Dialogue/",Character.Name.ToLower());
        //If file can't be found
        if(!System.IO.File.Exists(address))
        {
            //GD.Print("doesnt Exist");
            return null;
        }
        var file = System.IO.File.ReadAllText(address);
        return file.ToString();
    }

    public List<string> LoadLines(string file)
    {
        var lineList = new List<string>();
        //Extract each line in file
        foreach (Match match in Regex.Matches(file, @"(.*?)\n"))
        {
            //If line is just empty, don't inclue it in the list of lines
            if(match.ToString() != "\n")
            {
                var line = match.ToString();
                lineList.Add(line);
            }
        }
        return lineList;
    }

    public Dictionary<string,bool> LoadBools(List<string> Lines)
    {
        var injectable = new Dictionary<string,bool>();
        foreach (var line in Lines)
        {
            if(Regex.IsMatch(line, @"(?:--BOOL)"))
            {
                var Boolean = Regex.Match(line, @"\((.*?)\)").ToString();
                Boolean = ShaveAndRemoveWhitespaces(Boolean);
                injectable.Add(Boolean,false);

                //GD.Print("Bool Identified: " + Boolean);
            }
        }

        return injectable;
    }
    public Dictionary<string,int> LoadInts(List<string> Lines)
    {
        var injectable = new Dictionary<string,int>();
        foreach (var line in Lines)
        {
            if(Regex.IsMatch(line, @"(?:--INT)"))
            {
                var Interger = Regex.Match(line, @"\((.*?)\)").ToString();
                var IntName = Regex.Match(Interger,(@"\((.*?)\=")).ToString();
                IntName = ShaveAndRemoveWhitespaces(IntName);
                var IntNum = Regex.Match(Interger,(@"\=(.*?)\)")).ToString();
                IntNum = ShaveAndRemoveWhitespaces(IntNum);

                injectable.Add(IntName,System.Int16.Parse(IntNum));

                //GD.Print("Int Identified: " + IntName + " " + System.Int32.Parse(IntNum).ToString());            
            }
        }

        return injectable;
    }

    //I recognise this function is ridiculous however I have no idea why the Regex capture *includes* the limiters, so this is necessary
    //It also means if the user includes whitespaces, they'll be handled
    private static string ShaveAndRemoveWhitespaces(string desirable)
    {
        //GD.Print("To strip: " + desirable.ToString());

        List<char> StripChar = new List<char>(desirable.ToString().ToCharArray());
        StripChar.RemoveAt(0);
        StripChar.RemoveAt(StripChar.Count - 1);
        var CleanString = new string(StripChar.ToArray());
        CleanString = String.Concat(CleanString.Where(c => !Char.IsWhiteSpace(c)));
        return CleanString;
    }

}
