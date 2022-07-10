 ----------- ReadMe -----------

 Steps to integrate the Dialogue Manager into a new project:
 1. Create functions under Body_Entered and Body_Exited in the
 player's Area2D node.

 2. Add this block of code to player script;
    public void _On_Detection_Entered(StaticBody2D body)
    {
        DM.AddNPC(body);
        GD.Print("on body entered " + body.Name);
    }
    public void _On_Detection_Exited(StaticBody2D body)
    {
        DM.PopNPC(body);
        GD.Print("on body exited " + body.Name);
    }

3. Add empty node with DialogueManager.cs to scene

4. Add empty node called 'Targets' as child of each NPC, with
two Node2D nodes. First child will position Emotes, second will
position Text.

5. Create folder called 'Dialogue' inside the Assets folder.

