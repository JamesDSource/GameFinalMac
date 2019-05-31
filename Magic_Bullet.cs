using Godot;
using System;

public class Magic_Bullet : Area2D
{
	[Signal]
	delegate void attack(int damage); 
    private int speed = 1000;
	private int damage = 30;
	private float reduction_time = (float)0.5;
	private int damage_reduction = 5;
	private bool hitting = false;
	
	private Vector2 velocity = new Vector2();

	public void start(Vector2 pos, Vector2 dir)
	{
		Position = pos;
		Rotation = dir.Angle();
		velocity = dir * speed;
		GetNode<Timer>("damage_reduction_timer").WaitTime = reduction_time;
		GetNode<Timer>("damage_reduction_timer").Start();
	}
	public override void _Process(float delta)
	{
		if(!hitting)
		{Position += velocity * delta;}
		if(damage <= 0)
		{explode();}
	}
	public void explode()
	{
		GetNode<AnimatedSprite>("bullet_sprite").Play("Hit");
		hitting = true;
	}

	private void _on_Magic_Bullet_body_entered(object body)
	{
	    if(body.ToString() != "player" && body.ToString() != "Orb")
		{explode();}
		if(body.ToString() == "Warden")
		{Warden warden = (Warden)body;
		warden.take_damage(damage);}
	}
	private void _on_damage_reduction_timer_timeout()
	{
	    if(damage > 0)
		{damage = damage - damage_reduction;}
	}
	private void _on_bullet_sprite_animation_finished()
	{
	    QueueFree();
	}
}