using Godot;
using System;
using System.Collections.Generic;

public class NPCs
{
    public List<int> Ints;
    public List<bool> Bools;

    public Node Character;
    public string TextFile;
    public List<string> Lines;
    public int TimesTalkedTo = 0;

    public void SetCharacter(Node node)
    {
        Character = node;
    }
    public Node GetCharacter()
    {
        return Character;
    }

    public void SetTextFile(string file)
    {
        TextFile = file;
    }
    public string GetTextFile()
    {
        return TextFile;
    }

    public void SetLines(List<string> list)
    {
        foreach (var line in list)
        {
            Lines = list;
        }
    }
    public List<string> GetLines()
    {
        return Lines;
    }

    public void SetTimesTalkedTo(int count)
    {
        TimesTalkedTo = count;
    }
    public int GetTimesTalkedTo()
    {
        return TimesTalkedTo;
    }
}
