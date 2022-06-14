using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public class PlayerMovement : KinematicBody2D
{
    private Node2D Polygons;
    public KinematicBody2D Player;
    private Skeleton2D Skeleton;
	private AnimationTree AnimTree;
	private float max_speed = 180;
    private float acceleration = 200;
    public Vector2 velocity;
    private float dir_poly;
    private float dir_skele;
    private Tuple<Vector2, Vector2> PolySkeleScales;
    private Node current_grab;

    private DialogueManager DM;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    { 
        DM = GetNode<DialogueManager>("../Dialogue Manager");
        
        Player = GetNode<KinematicBody2D>(".");
        Polygons = GetNode<Node2D>("Player Polygons");
        Skeleton = GetNode<Skeleton2D>("Skeleton Frog");
        AnimTree = GetNode<AnimationTree>("Animations/AnimationTree");
        PolySkeleScales = new Tuple<Vector2,Vector2>(
            new Vector2(Polygons.Scale.x,Polygons.Scale.y),
            new Vector2(Skeleton.Scale.x,Skeleton.Scale.y));

        dir_poly = PolySkeleScales.Item1.x;
        dir_skele = PolySkeleScales.Item2.x;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
	{
        //velocity.y += delta * gravity;

        if (Input.IsActionPressed("move_left") && velocity.x >= -max_speed)
        {
            dir_poly    = -PolySkeleScales.Item1.x;
            dir_skele   = -PolySkeleScales.Item2.x;
            velocity.x -= acceleration * delta;
        }
        else if (Input.IsActionPressed("move_right") && velocity.x <= max_speed)
        {
            dir_poly    = PolySkeleScales.Item1.x;
            dir_skele   = PolySkeleScales.Item2.x;
            velocity.x += acceleration * delta;
        }
        else
        {
            velocity.x = Mathf.Lerp(velocity.x, 0, .1F);
        }
        
        Polygons.Scale = new Vector2(dir_poly, PolySkeleScales.Item1.y);
        Skeleton.Scale = new Vector2(dir_skele, PolySkeleScales.Item2.y);
        
        var animVelocity = (velocity.x/max_speed);
        if (animVelocity < 0)
        {
            animVelocity *= -1;
        }

        // We don't need to multiply velocity by delta because "MoveAndSlide" already takes delta time into account.
        AnimTree.Set("parameters/Idle + Walking Blend Tree/Blend2 2/blend_amount", animVelocity);
        // The second parameter of "MoveAndSlide" is the normal pointing up.
        // In the case of a 2D platformer, in Godot, upward is negative y, which translates to -1 as a normal.
        MoveAndSlide(velocity);
        //GD.Print("Velocity: " + velocity);
        
        if(Input.IsActionJustPressed("ui_accept"))
        {
            DM.ReadLine();
        }
    }
    
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
}
