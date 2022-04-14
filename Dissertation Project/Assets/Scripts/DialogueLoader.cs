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
    private RegexCheck RC;
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
        var BoolDictionary  = new Dictionary<string,bool>();
        foreach (var line in Lines)
        {
            if(RC.BOOL.IsMatch(line))
            {
                var Boolean = Regex.Match(line, @"\((.*?)\)").ToString();
                Boolean = PopFirstLastandWhitespaces(Boolean);
                BoolDictionary.Add(Boolean,false);

                //GD.Print("Bool Identified: " + Boolean);
            }
        }

        return BoolDictionary;
    }
    public Dictionary<string,int> LoadInts(List<string> Lines)
    {
        var IntDictionary = new Dictionary<string,int>();
        foreach (var line in Lines)
        {
            if(RC.INT.IsMatch(line))
            {
                var Integer = Regex.Match(line, @"\((.*?)\)").ToString();
                var IntName = Regex.Match(Integer,(@"\((.*?)\=")).ToString();
                IntName = PopFirstLastandWhitespaces(IntName);
                var IntNum = Regex.Match(Integer,(@"\=(.*?)\)")).ToString();
                IntNum = PopFirstLastandWhitespaces(IntNum);

                IntDictionary.Add(IntName,System.Int16.Parse(IntNum));

                GD.Print("Int Identified: " + IntName + " " + System.Int32.Parse(IntNum).ToString() + " from " + Integer.ToString());            
            }
        }

        return IntDictionary;
    }

    //I recognise this function is ridiculous however I have no idea why the Regex capture *includes* the limiters, so this is necessary
    //It also means if the user includes whitespaces, they'll be handled
    public string PopFirstLastandWhitespaces(string text)
    {
        //GD.Print("To strip: " + desirable.ToString());

        List<char> StripChar = new List<char>(text.ToString().ToCharArray());
        StripChar.RemoveAt(0);
        StripChar.RemoveAt(StripChar.Count - 1);
        var AlteredText = new string(StripChar.ToArray());
        AlteredText = String.Concat(AlteredText.Where(c => !Char.IsWhiteSpace(c)));
        return AlteredText;
    }

}
