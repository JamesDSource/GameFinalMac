
using Godot;
using System;

public class Orb : KinematicBody2D
{
	[Signal]
	delegate void can_pick_up();
	private player playerChar;
	private bool can_pickup = false;
    private bool picked_up = false; 
	private bool left = false;
	private bool needs_to_move = false; 
	private float target_x;
	private float target_y;
	private int movement_speed = 200; 
	private Vector2 velocity = new Vector2();
	public override void _Ready()
    {
		GetNode<Sprite>("PickUp").Hide();
	}
	public override void _PhysicsProcess(float delta)
	{
		if(picked_up)
		{
			target_y = playerChar.GetPosition().y; 
			target_x = playerChar.GetPosition().x; 
			if(playerChar.get_can_move() && playerChar.get_orb_move())
				{
					if(GetPosition().y < target_y)
					{velocity.y = movement_speed;}
					else if(GetPosition().y > target_y)
					{velocity.y = -movement_speed;}
					else
					{velocity.y = 0;}
					if(GetPosition().x < target_x)
					{velocity.x = movement_speed;}
					else if(GetPosition().x > target_x)
					{velocity.x = -movement_speed;}
					else
					{velocity.x = 0;}
				}
			else
				{
					velocity.y = 0; 
					velocity.x = 0;
				}
				MoveAndSlide(velocity);
		}
	}
	public override void _Process(float delta)
	{
		if(can_pickup)
		{EmitSignal(nameof(can_pick_up));}
	}
	
	//Signals
	private void _on_Detection_body_entered(object body)
	{
	    if(picked_up == false && body.ToString() == "player")
		{GetNode<Sprite>("PickUp").Show();
		can_pickup = true;
		playerChar = (player)body;}
	}
	private void _on_Detection_body_exited(object body)
	{
	    if(body.ToString() == "player")
		{GetNode<Sprite>("PickUp").Hide();
		can_pickup = false;}
	}
	private void _on_player_is_picking_up()
	{
		picked_up = true;
		GetNode<Sprite>("PickUp").Hide();
	}
}