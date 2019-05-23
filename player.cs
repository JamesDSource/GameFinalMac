using Godot;
using System;


public class Attack
{
	//vars
	private int damage;
	private float cooldown;
	private string animation; 
	//constructer
	public Attack(int damage, float cooldown, string animation)
	{
		this.cooldown = cooldown;
		this.damage = damage;
		this.animation = animation; 
	}
	
	//getters
	public float getCooldown()
	{return cooldown;}
	public int getDamage()
	{return damage;}
	public string getAnimation()
	{return animation;}
	
	//setters
	public void setCooldown(float cooldown)
	{this.cooldown = cooldown;}
	public void setDamage(int damage)
	{this.damage = damage;}
	public void setAnimation(string animation)
	{this.animation = animation;}
	
}



public class player : KinematicBody2D
{	[Signal]
	delegate void is_picking_up();
	//vector
	private Vector2 velocity = new Vector2();
	//ints
	private int health = 100;
	private int movement_speed = 500;
	private int jump_speed = 1500; 
	private int gravity = 50;
	//bools
	private bool can_move = true;
	private bool is_moving_sideways = false;
	private bool left = false;
	private bool airborn = false; 
	private bool orb_move = true; 
	
	
	
	public override void _Ready()
    {
    }
	public override void _PhysicsProcess(float delta)
	{		
		//movement
		if(can_move)
		{
			velocity.y += gravity; 
			if(Input.IsActionPressed("ui_move_right"))
			{velocity.x = movement_speed;
			left = false;}
			else if(Input.IsActionPressed("ui_move_left"))
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
	}
	
	
	//getters
	public bool get_can_move()
		{return can_move;}
	public bool get_orb_move()
		{return orb_move;}
	
	
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
		{orb_move = false;}
	}
	private void _on_OrbArea_body_exited(object body)
	{
    	if(body.ToString() == "Orb")
		{orb_move = true;}
	}
}