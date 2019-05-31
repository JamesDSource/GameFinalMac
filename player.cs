using Godot;
using System;
public class player : KinematicBody2D
{
	[Signal]
	delegate void is_picking_up();
	//vector
	public Vector2 velocity = new Vector2();
	//ints
	private int health = 100;
	private int movement_speed = 500;
	private int jump_speed = 900; 
	private int gravity = 50;
	private int max_speed = 900;
	private int base_speed = 500;
	private int speed_increase = 50; 
	//bools
	private bool can_shoot = true;
	private bool can_move = true;
	private bool is_moving_sideways = false;
	private bool left = false;
	private bool airborn = false; 
	private bool orb_move = true; 
	//float
	private float speed_buidup_time = 1;
	
	
	public override void _Ready()
    {
		GetNode<Timer>("Speed").SetWaitTime(speed_buidup_time);
		GetNode<Timer>("Speed").Start();
    }
	public override void _PhysicsProcess(float delta)
	{		
		//movement
		if(can_move)
		{
			velocity.y += gravity; 
			if(Input.IsActionPressed("ui_walk_right"))
			{velocity.x = movement_speed;
			left = false;}
			else if(Input.IsActionPressed("ui_walk_left"))
			{velocity.x = -movement_speed;
			left = true;}
			else
			{velocity.x = 0;}
			if(IsOnWall() && Input.IsActionJustPressed("ui_jump"))
			{airborn = true;can_move = false;}
		}
		else
		{velocity.x = 0; velocity.y = 0;}
		if(GetNode<AnimatedSprite>("playerSprite").GetAnimation().Equals("left_jump") || GetNode<AnimatedSprite>("playerSprite").GetAnimation().Equals("right_jump"))
			{if(GetNode<AnimatedSprite>("playerSprite").GetFrame() == 3)
				{velocity.y = -jump_speed;}}
		velocity = MoveAndSlide(velocity);
	
	}
	public override void _Process(float delta)
	{
		if(velocity.x != 0)
		{is_moving_sideways = true;}
		else
		{is_moving_sideways = false;}
		if(airborn && left)
		{playAnimation("left_jump");}
		else if(airborn && !left)
			{playAnimation("right_jump");}
		else if(is_moving_sideways && left)
			{playAnimation("left_walk");}
		else if(is_moving_sideways && !left)
			{playAnimation("right_walk");}
		else if(!is_moving_sideways && left)
			{playAnimation("left_idle");}
		else if(!is_moving_sideways && !left)
			{playAnimation("right_idle");}
			
			
		if(health <= 0)
			{Hide();}
		

	}
	
	
	//getters
	public bool get_can_move()
		{return can_move;}
	public bool get_orb_move()
		{return orb_move;}
	public int get_movement_speed()
		{return movement_speed;}
	
	public void damage_taken(int damage)
	{health = health - damage;}
	
	public void playAnimation(string animation)
	{GetNode<AnimatedSprite>("playerSprite").Play(animation);}
	private void _on_playerSprite_animation_finished()
	{
	    if(GetNode<AnimatedSprite>("playerSprite").GetAnimation().Equals("left_jump") || GetNode<AnimatedSprite>("playerSprite").GetAnimation().Equals("right_jump"))
		{airborn = false;
		can_move = true;}
	}
	private void _on_Orb_can_pick_up()
	{
	    if(Input.IsActionPressed("ui_interact"))
		{EmitSignal(nameof(is_picking_up));}
	}
	private void _on_OrbArea_body_entered(object body)
	{
		if(body.ToString() == "Orb")
		{orb_move = false;
		AddCollisionExceptionWith((Node)body);}
	}
	private void _on_OrbArea_body_exited(object body)
	{
    	if(body.ToString() == "Orb")
		{orb_move = true;}
	}
	private void _on_Speed_timeout()
	{
	    if(velocity.x > 0 && velocity.x < max_speed)
		{movement_speed = movement_speed + speed_increase;}
		else if(velocity.x < 0 && velocity.x > -max_speed)
		{movement_speed = movement_speed + speed_increase;}
		else if(velocity.x == 0)
		{movement_speed= base_speed;}
	}
}