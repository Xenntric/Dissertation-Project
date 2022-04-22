using Godot;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
public struct RegexCheck
{   
    DialogueLogic logic;

    public enum Parse
    {
        DialogueEmote,
        Dialogue,
        End,
        If,
        Set,
        SkipLine,
        PlayAnimation,
        ReParseLine,

    } 

    public Regex DIALOGUE
    {
        get{return new Regex(@"(^[^|]*)");}
    }
    public Regex EMOTE
    {
        get{return new Regex(@"\|(.*?)\n");}
    }
    public bool HasEMOTE(string line)
    {
        if(Regex.Match(line,@"[|]+").Success)
        {
            return true;
        }
        return false;
    }
    public bool Has(string stringToCheck, string text)
    {
        stringToCheck = Regex.Unescape(stringToCheck);
        if(Regex.IsMatch(text,@"[{stringToCheck}]"))
        {
            return true;
        }
        return false;
    }

    public Regex FILENAME
    {
        get{return new Regex(@"[^\/]+$");}
    }
    public Regex LINE
    {
        get{return new Regex(@"(.*?)\n");}
    }


    public string CaptureParentheses(string line)
    {
        line = Regex.Match(line, @"\((.*?)\)").ToString();
        line = logic.RemoveWhitespace(logic.PopFirst(logic.PopLast(line)));
        return line;
    }
    public string CaptureWithin(string start, string end, string line)
    {
        GD.Print("Capture from " + start + " to " + end + " in " + line);
        if(start!="^")
        {
             start = Regex.Escape(start);
        }
        else
        {
            start = Regex.Unescape(start);
        }

        if(end!="$")
        {
            end = Regex.Escape(end);
        }
        else
        {
            end = Regex.Unescape(end);            
        }

        line = (Regex.Match(line, (start+"(.*?)"+end)).ToString());
        
        return line;
    }

    //End of Text File
    public Regex END
    {
        get{return new Regex(@"^(?:END)\n");}
    }
    
    //Bool Conditional
    public Regex BOOL
    {
        get{return new Regex(@"^(?:>>BOOL)");}
    }
    //Integer Conditional
    public Regex INT
    {
        get{return new Regex(@"^(?:>>INT)");}
    }

    //If Command
    public Regex IF
    {
        get{return new Regex(@"^(?:--IF)");}
    }
    //Set conditional to
    public Regex SET
    {
        get{return new Regex(@"^(?:--SET)");}
    }
    public Regex PLAY
    {
        get{return new Regex(@"^(?:--PLAY)");}
    }
}