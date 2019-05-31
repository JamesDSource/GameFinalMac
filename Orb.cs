
using Godot;
using System;

public class Orb : KinematicBody2D
{
	[Signal]
	delegate void can_pick_up();
	[Signal]
	delegate void projectile(PackedScene bullet, Vector2 dir, Vector2 pos);
	private PackedScene bullet = GD.Load<PackedScene>("res://Scenes/Magic_Bullet.tscn");
	private player playerChar;
	private bool can_attack = true;
	private bool can_pickup = false;
    private bool picked_up = false; 
	private bool left = false;
	private bool can_move_x = true;
	private bool can_move_y = true; 
	private float target_x;
	private float target_y;
	private float attack_speed = (float)0.5;
	private int movement_speed = 400; 
	private Vector2 velocity = new Vector2();
	public override void _Ready()
    {
		GetNode<AnimatedSprite>("PickUp").Hide();
		GetNode<Timer>("attack_speed").Start();
	}
	public override void _PhysicsProcess(float delta)
	{
		if(picked_up)
		{
			target_y = playerChar.GlobalPosition.y; 
			target_x = playerChar.GlobalPosition.x; 
			//GD.Print(playerChar.get_orb_move());
			if(playerChar.get_can_move() && playerChar.get_orb_move())
				{
					if(Math.Abs(GlobalPosition.y - target_y) <= 30)
					{can_move_y = false;}
					if(Math.Abs(GlobalPosition.x - target_x) <= 30)
					{can_move_x = false;}
					if(can_move_y)
					{
						if(GlobalPosition.y < target_y)
						{velocity.y = movement_speed;}
						else if(GlobalPosition.y > target_y)
						{velocity.y = -movement_speed;}
						else
						{velocity.y = 0;}
					}
					else
					{velocity.y = 0;}
					if(can_move_x)
					{
						if(GlobalPosition.x < target_x)
						{velocity.x = movement_speed;}
						else if(GlobalPosition.x > target_x)
						{velocity.x = -movement_speed;}
						else
						{velocity.x = 0;}
					}
					else
					{velocity.x = 0;}
					if(playerChar.velocity.y != 0)
					{can_move_y = true;}
					if(playerChar.velocity.x != 0)
					{can_move_x = true;}
					
					movement_speed = playerChar.get_movement_speed() - 100; 
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
		//Signal emmitions
		if(can_pickup)
		{EmitSignal(nameof(can_pick_up));}
		
		//left or not
		if(velocity.x < 0)
		{left = true;}
		else if(velocity.x > 0)
		{left = false;}
		
		//Animations
		GetNode<Sprite>("Puple").LookAt(GetGlobalMousePosition());
		
		if(picked_up && Input.IsActionJustPressed("ui_shoot") && can_attack)
		{
			Vector2 dir = new Vector2(1,0).Rotated(GetNode<Sprite>("Puple").GlobalRotation);
			EmitSignal("projectile", bullet, dir, GetNode<Sprite>("Puple").GetNode<Position2D>("Aim").GlobalPosition);
			can_attack = false;
		}
	}
	
	//Signals
	private void _on_Detection_body_entered(object body)
	{
	    if(picked_up == false && body.ToString() == "player")
		{GetNode<AnimatedSprite>("PickUp").Show();
		GetNode<AnimatedSprite>("PickUp").Play("BaDing");
		can_pickup = true;
		playerChar = (player)body;}
	}
	private void _on_Detection_body_exited(object body)
	{
	    if(body.ToString() == "player")
		{GetNode<AnimatedSprite>("PickUp").Hide();
		can_pickup = false;} 
	}
	private void _on_player_is_picking_up()
	{
		picked_up = true;
		GetNode<AnimatedSprite>("PickUp").Hide();
	}
	private void _on_attack_speed_timeout()
	{
	    can_attack = true;
		GetNode<Timer>("attack_speed").Start();
	}
}
