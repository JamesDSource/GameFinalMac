
using Godot;
using System;

public class Orb : KinematicBody2D
{
    private bool picked_up = false; 
	public override void _Ready()
    {
		GetNode<Sprite>("PickUp").Hide();
	}
	public override void _PhysicsProcess(float delta)
	{
		
	}
	public override void _Process(float delta)
	{
		
	}
	
	
	private void _on_Detection_body_entered(object body)
	{
	    if(picked_up == false && body.ToString() == "player")
		{GetNode<Sprite>("PickUp").Show();}
	}
	
	
	private void _on_Detection_body_exited(object body)
	{
	    if(picked_up = false && body.ToString() == "player")
		{GetNode<Sprite>("PickUp").Hide();}
	}
}