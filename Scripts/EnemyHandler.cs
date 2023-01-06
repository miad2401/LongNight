using Godot;
using System;

public class EnemyHandler : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	[Export] private int hp = 5;
	[Export] private int def = 3;
	[Export] private int agi = 2;

	[Signal] public delegate void attackPlayer(int roll);


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect(nameof(attackPlayer), GetNode("/root/Scene/Char"), "takeDamage");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	//Handle enemy turns
	public void turn()
	{
		//if not close enough to attack, move to attack then attack
		Node2D player = GetNode<Node2D>("/root/Scene/Char");
		Vector2 playerPos = player.GlobalPosition;
		Vector2 pos = this.GlobalPosition;
		//True = enemy is in range to make attack
		bool inRange = (playerPos.x == pos.x - 64 && playerPos.y == pos.y - 32) ||
			(playerPos.x == pos.x + 64 && playerPos.y == pos.y + 32) ||
			(playerPos.x == pos.x - 64 && playerPos.y == pos.y + 32) ||
			(playerPos.x == pos.x + 64 && playerPos.y == pos.y - 32);

		System.Threading.Thread.Sleep(200); //wait for 2 seconds to give illusion of thinking

		if (inRange)
		{
			GD.Print("Player pos:", playerPos);
			GD.Print("Enemy pos:", pos);
			attack();
		} else
		{
			GD.Print("Move command issued");
			move();
		}
	}

	public void move()
	{
		Node2D player = GetNode<Node2D>("/root/Scene/Char");
		Vector2 playerPos = player.GlobalPosition;
		Vector2 pos = this.GlobalPosition;

		//Get how many spaces away we are
		int tilesX = (int)(playerPos.x - pos.x)/64;
		int tilesY = (int)(playerPos.y - pos.y)/32;
		int tilesAway = tilesX + tilesY;
		GD.Print("Tiles away: " + tilesAway);

		if (tilesX > tilesY)
		{
			pos.x = pos.x + (Math.Sign(tilesX) * 64);
			this.GlobalPosition = pos;
		} else
		{
			pos.y = pos.y + (Math.Sign(tilesY) * 32);
			this.GlobalPosition = pos;
		}
		
	}

	public void attack()
	{
		//ideally play anim attacking player in whatever direction the player is in relation to the enemy
		Random rnd  = new Random();
		int roll = rnd.Next(1, 11);
		GD.Print("Attack Roll: " + roll);
		EmitSignal(nameof(attackPlayer), roll);
	}
}
