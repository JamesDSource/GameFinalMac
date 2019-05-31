using Godot;
using System;

public class Baseball : Area2D
{
	[Signal]
	delegate void attack(int damage); 
    private int speed = 500;
	private int damage = 20;
	
	private Vector2 velocity = new Vector2();

	public void start(Vector2 pos, Vector2 dir)
	{
		Position = pos;
		Rotation = dir.Angle();
		velocity.x = speed;
		velocity.y = speed;
	}
	public override void _Process(float delta)
	{
	}

	private void _on_Magic_Bullet_body_entered(object body)
	{
	    if(body.ToString() == "player")
		{player playerChar = (player)body;
		playerChar.damage_taken(damage);}
		bounce();
	}
	public void bounce()
	{
		velocity.y = velocity.y * -1;
		velocity.x = velocity.x * -1;
	}
}
