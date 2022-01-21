using Godot;
using System;

public class Scripts : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export]
	public KinematicBody2D Player;

	[Export]
	public float Speed = 4;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Player = (KinematicBody2D)FindNode("PLayer");
		Player = GetNode<KinematicBody2D>("/root/World/Player");
		
		//Player = (KinematicBody2D)FindNode("Player");

		if((Sprite)GetNode("/root/World/Environment Nodes/StaticBody2D/Floor") is Sprite)
		{
			GD.Print("ding");
		}
		if (Player is KinematicBody2D)
		{
			GD.Print("dong");
		}
		
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsKeyPressed((int)KeyList.A))
		{

		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("move_right"))
		{
			 GD.Print("boot");
		}
		
		

	}
}
