using Godot;
using System;

public class Warden : KinematicBody2D
{
	[Signal]
	delegate void projectile(PackedScene ball, Vector2 dir, Vector2 pos);
	[Signal]
	delegate void hit();
	private PackedScene ball = GD.Load<PackedScene>("res://Scenes/Baseball.tscn");
	private player playerChar;
	private string state;
	private int health = 400;
	private int movement_speed = 500; 
	private int melee_damage = 20;
	private int sum = 0;
	private float target_x;
	private float melee_delay = (float)0.75;
	private bool left = true;
	private bool player_close = false;
	private bool can_move_x = true;
	private bool in_range = false;
	private bool attacking = false;
	private bool can_swing = true;
	private bool has_hit = false;
	private Vector2 velocity = new Vector2();
	Random rnd = new Random();
    public override void _Ready()
    {
		GetNode<Timer>("Melee_Timer").SetWaitTime(melee_delay);
		GetNode<Timer>("Melee_Timer").Start();
		
    }
	public override void _PhysicsProcess(float delta)
	{
		MoveAndSlide(velocity);
		velocity.y = 30;
		if(playerChar != null)
		{target_x = playerChar.GlobalPosition.x;}
		if(state == null)
		{
			sum = 0;
			if(left)
			{playAnimation("idle_left");}
			else
			{playAnimation("idle_right");}
			if(playerChar != null)
			{
				if(player_close && rnd.Next(1,11) <= 5)
				{state = "melee";}
				else if(player_close)
				{state = "back_away";}
				else if(rnd.Next(1,11) <= 5)
				{state = "baseball";}
				else if(!player_close)
				{state = "melee";}
			}
		}
		else if(state == "melee")
		{
			if(!in_range)
			{
				if(GlobalPosition.x < target_x)
				{velocity.x = movement_speed;
				left = false;}
				else if(GlobalPosition.x > target_x)
				{velocity.x = -movement_speed;
				left = true;}
				else
				{velocity.x = 0;}
			}
			else
			{
				attacking = true;
				velocity.x = 0;
				if(can_swing)
				{	
					if(left)
					{playAnimation("swing_left");}
					else
					{playAnimation("swing_right");}
				}
			}
		}
		else if(state == "back_away")
		{
			if(GlobalPosition.x < target_x)
			{velocity.x = -movement_speed;
			left = false;}
			else if(GlobalPosition.x > target_x)
			{velocity.x = movement_speed;
			left = true;}
			if(sum >= rnd.Next(30000, 50001))
			{state = null;}
			sum = sum + movement_speed;
		}
		else if(state == "baseball")
		{
			//Vector2 dir = new Vector2(1,0).Rotated(GetNode<Sprite>("pointer").GlobalRotation);
			//EmitSignal("projectile", ball, dir, GetNode<Sprite>("pointer").GetNode<Position2D>("Aim").GlobalPosition);
			state = "melee";
		}
	}
	public override void _Process(float delta)
	{
		if(health <= 0)
		{QueueFree();}
		if(playerChar != null)
		{
			//GetNode<Sprite>("pointer").LookAt(playerChar.GlobalPosition);
			if(GetNode<AnimatedSprite>("jane").GetAnimation() == "swing_left" && GetNode<AnimatedSprite>("jane").GetFrame() < 17 && GetNode<AnimatedSprite>("jane").GetFrame() > 12 && target_x < GlobalPosition.x && in_range && can_swing) 
			{
				
				playerChar.damage_taken(melee_damage);
				attacking = false;
				can_swing = false;
				if(rnd.Next(1, 11) <= 5)
					{state = "back_away";}
			}
			else if(GetNode<AnimatedSprite>("jane").GetAnimation() == "swing_right" && GetNode<AnimatedSprite>("jane").GetFrame() < 17 && GetNode<AnimatedSprite>("jane").GetFrame() > 12 && target_x > GlobalPosition.x && in_range && can_swing) 
			{
				playerChar.damage_taken(melee_damage);
				attacking = false;
				can_swing = false;
				if(rnd.Next(1, 11) <= 5)
					{state = "back_away";}
			}
		}
		if(!in_range && velocity.x != 0)
		{
			if(!left)
			{
				playAnimation("walk_right");
			}
			else
			{
				playAnimation("walk_left");
			}
		}
		else if(velocity.x == 0 && !attacking)
		{
			if(!left)
			{
				playAnimation("idle_right");
			}
			else
			{
				playAnimation("idle_left");
			}
		}
		
		if(health <= 0)
		{QueueFree();}
		
		//if(has_hit)
		//{
		//if(left)
		//{playAnimation("hit_left");}
		//else
		//{playAnimation("hit_right");}
		//}
		
		//if(GetNode<AnimatedSprite>("jane").GetAnimation() == "hit_right" || GetNode<AnimatedSprite>("jane").GetAnimation() == "hit_left")
			//if(GetNode<AnimatedSprite>("jane").GetFrame() == 16)
			//{EmitSignal("hit");}
			//else if(GetNode<AnimatedSprite>("jane").GetFrame() == 18)
			//{state = null;}
		
	}
	public void play_ball()
	{
		//has_hit = true;
		//velocity.x = 0;
		//Vector2 dir = new Vector2(1,0).Rotated(GetNode<Sprite>("pointer").GlobalRotation);
		//EmitSignal("projectile", ball, dir, GetNode<Sprite>("pointer").GetNode<Position2D>("Aim").GlobalPosition);
	}
	public void take_damage(int damage)
	{health = health - damage;}
	public void playAnimation(string animation)
	{
		if(GetNode<AnimatedSprite>("jane").GetAnimation() == "swing_right" || GetNode<AnimatedSprite>("jane").GetAnimation() == "swing_left")
			{if(GetNode<AnimatedSprite>("jane").GetFrame() == 18)
				{GetNode<AnimatedSprite>("jane").Play(animation);}}
		else
		{GetNode<AnimatedSprite>("jane").Play(animation);}
	}
	//Signals
	private void _on_Detection_body_entered(object body)
	{
	    if(body.ToString() == "player")
		{playerChar = (player)body;
		AddCollisionExceptionWith((Node)body);}
		else if(body.ToString() == "Orb")
		{AddCollisionExceptionWith((Node)body);}
	}
	private void _on_Closeness_body_entered(object body)
	{
	    if(body.ToString() == "player")
		{player_close = true;}
	}
	private void _on_Closeness_body_exited(object body)
	{
	    if(body.ToString() == "player")
		{player_close = false;}
	}
	private void _on_Melee_Range_body_entered(object body)
	{
	    if(body.ToString() == "player")
		{in_range = true;}
	}
	private void _on_Melee_Range_body_exited(object body)
	{
	    if(body.ToString() == "player")
		{in_range = false;}
	}
	private void _on_Melee_Timer_timeout()
	{
		can_swing = true;
	    GetNode<Timer>("Melee_Timer").Start();
	}
}