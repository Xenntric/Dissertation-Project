using Godot;
using System;

public class DialogueUI : Control
{
    private Control ControlNode;

    public Font font
    {
        get {return font;}
        set {font = value;}
    }
    public Vector2 position
    {
        get {return position;}
        set {position = value;}
    }
    public string dialogue
    {
        get {return dialogue;}
        set {dialogue = value;}
    }
    
    /* public override void _Ready()
    {
        GD.Print("RunUI");
    }
    override public void _Draw()
    {
        ControlNode.DrawString(font, position, dialogue, null, -1);
    }

    public void SetFont(Font newFont)
    {
        font = newFont;
    }
    public Font GetFont()
    {
        return font;
    }

    public void Setposition(Vector2 newPosition)
    {
        position = newPosition;
    }
    public Vector2 Getposition()
    {
        return position;
    }

    public void SetDialogue(string newDialogue)
    {
        dialogue = newDialogue;
    }
    public string GetDialogue()
    {
        return dialogue;
    } */
}
