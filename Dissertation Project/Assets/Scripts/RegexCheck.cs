using Godot;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
public struct RegexCheck
{
    public enum Parse
    {
        DialogueEmote,
        Dialogue,
        End,
        If,
        Set,
        SkipLine,
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

    //End of Text File
    public Regex END
    {
        get{return new Regex(@"^(?:END)\n");}
    }

    /* //Start of Loop
    public Regex SLOOP
    {
        get{return new Regex(@"^(?:SLOOP)");}
    }
    //End of Loop
    public Regex ELOOP
    {
        get{return new Regex(@"^(?:ELOOP)");}
    }
    //Break Loop
    public Regex BLOOP
    {
        get{return new Regex(@"^(?:BLOOP)");}
    } */
    
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
}