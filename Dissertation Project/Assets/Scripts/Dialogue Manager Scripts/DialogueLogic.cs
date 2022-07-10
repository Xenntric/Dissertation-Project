using Godot;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

public struct DialogueLogic
{
    
    public string PopFirst(string text)
    {
        List<char> StripChar = new List<char>(text.ToCharArray());
        StripChar.RemoveAt(0);
        return new string(StripChar.ToArray());
    }
    public string PopLast(string text)
    {
        List<char> StripChar = new List<char>(text.ToCharArray());
        StripChar.RemoveAt(StripChar.Count - 1);
        return new string(StripChar.ToArray());
    }
    public string RemoveWhitespace(string text)
    {        
        text = String.Concat(text.Where(c => !Char.IsWhiteSpace(c)));
        return text;
    }
}
