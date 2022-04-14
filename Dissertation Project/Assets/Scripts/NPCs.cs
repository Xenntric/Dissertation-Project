using Godot;
using System;
using System.Collections.Generic;

public class NPCs
{
    public Dictionary<string,int> Ints;
    public Dictionary<string,bool> Bools;

    public Node Character;
    public string TextFile;
    public List<string> Lines;
    public int TimesTalkedTo = 0;
}
