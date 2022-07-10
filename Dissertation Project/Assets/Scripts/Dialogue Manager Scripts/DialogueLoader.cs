using Godot;
using System;
using System.IO;
using System.Text.RegularExpressions;

using System.Collections.Generic;
using System.Linq;

public struct DialogueLoader
{
    private RegexCheck RC;
    private DialogueLogic logic;
    
    public string LoadFile(Node Character)
    {
        var address = System.IO.Path.Combine(@"Assets/Dialogue/",Character.Name);
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
        foreach (Match match in RC.LINE.Matches(file))
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
                var Boolean = RC.CaptureParentheses(line);
                BoolDictionary.Add(Boolean,false);
                GD.Print("Bool Identified: " + Boolean);
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
                var Integer = RC.CaptureParentheses(line);
                var IntName = logic.PopLast(RC.CaptureWithin("^","=",Integer));
                var IntNum = logic.PopFirst(RC.CaptureWithin("=","$", Integer));
                IntDictionary.Add(IntName,System.Int16.Parse(IntNum));

                GD.Print("Int Identified: " + IntName + " " + System.Int32.Parse(IntNum).ToString() + " from " + Integer.ToString());            
            }
        }

        return IntDictionary;
    }

}
