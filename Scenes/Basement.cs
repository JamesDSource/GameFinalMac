using Godot;
using System;

public class Basement : Node2D
{
	private bool hit = false;
    public override void _Ready()
    {
        
    }
	private void _on_Orb_projectile(PackedScene bullet, Vector2 dir, Vector2 pos)
	{
	    var _bullet = (Magic_Bullet)bullet.Instance();
		AddChild(_bullet);
		_bullet.start(pos, dir);
	}
	private void _on_Warden_projectile(PackedScene ball, Vector2 dir, Vector2 pos)
	{
	   var _ball = (Baseball)ball.Instance();
		AddChild(_ball);
		_ball.start(pos, dir);
	}

}
