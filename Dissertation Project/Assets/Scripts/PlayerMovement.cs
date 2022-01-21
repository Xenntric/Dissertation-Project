using Godot;
using System;

public class PlayerMovement : KinematicBody2D
{
    [Export]
	public KinematicBody2D Player;
    public Node2D Polygons;
    public Skeleton2D Skeleton;

	[Export]
	public float max_speed = 180;
    public float acceleration = 200;

    public Vector2 velocity;
    public float dir_poly;
    public float dir_skele;

    public AnimationTree AnimTree;
    private bool moving;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
        Player = GetNode<KinematicBody2D>("/root/World/Player");
        Polygons = GetNode<Node2D>("/root/World/Player/Player Polygons");
        Skeleton = GetNode<Skeleton2D>("/root/World/Player/Skeleton Frog");
        AnimTree = GetNode<AnimationTree>("/root/World/Player/Animations/AnimationTree");
        
        dir_poly = Polygons.Scale.x;
        dir_skele = Skeleton.Scale.x;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
	{
        //velocity.y += delta * gravity;

        if (Input.IsActionPressed("move_left") && velocity.x >= -max_speed)
        {
            dir_poly    = -.33F;
            dir_skele   = -1;
            velocity.x -= acceleration * delta;
        }
        else if (Input.IsActionPressed("move_right") && velocity.x <= max_speed)
        {
            dir_poly    = .33F;
            dir_skele   = 1;
            velocity.x += acceleration * delta;
        }
        else
        {
            velocity.x = Mathf.Lerp(velocity.x, 0, .1F);
        }
        
        Polygons.Scale = new Vector2(dir_poly, Polygons.Scale.y);
        Skeleton.Scale = new Vector2(dir_skele, Skeleton.Scale.y);
        
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
        GD.Print("Velocity: " + velocity);
	}
    public override void _Input(InputEvent inputEvent)
	{

    }

}
