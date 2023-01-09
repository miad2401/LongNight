using Godot;
using System;

public class EnemyHandler : Node2D
{

	[Export] private int hp = 5;
	[Export] private int def = 2;
	[Export] private int agi = 1;

	[Signal] public delegate void attackPlayer(int roll);
	[Signal] public delegate void changeText(Node toChange, String text);
	[Signal] public delegate void enemyKilled();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect(nameof(attackPlayer), GetNode("/root/Scene/Char"), "takeDamage");
		Connect(nameof(changeText), GetNode("/root/Scene/Char/Camera2D/Control"), "setText");
		Connect(nameof(enemyKilled), GetNode("/root/Scene/Char"), "updateKillCount");
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

	public void takeDamage(int amount)
	{
		Label combatLog = GetNode<Label>("/root/Scene/Char/Camera2D/Control/TopBar/CombatLog");
		Random rnd = new Random();

		if (rnd.Next(1, agi) < (amount / 2))
		{
			amount -= def;
			if (amount > 0)
			{
				EmitSignal(nameof(changeText), combatLog, "Combat Log: \n Player Hit!");
				hp -= amount;
				if (hp <= 0)
				{
					EmitSignal(nameof(changeText), combatLog, "Combat Log: \n Enemy Respawning!");
					
					//Respawn at random location
					Vector2 pos = this.GlobalPosition;
					TileMap map = GetNode<TileMap>("/root/Scene/Base");

					Vector2 enemyPosLocal = map.ToLocal(pos);
					Vector2 enemyTilePos = map.WorldToMap(enemyPosLocal);
					int randX = rnd.Next((int)enemyTilePos.x - 5, (int)enemyTilePos.x + 5);
					int randY = rnd.Next((int)enemyTilePos.y - 5, (int)enemyTilePos.y + 5);
					enemyTilePos.x = randX; enemyTilePos.y = randY;
					enemyPosLocal = map.MapToWorld(enemyTilePos);
					pos = map.ToGlobal(enemyPosLocal);
					//normalize for iso grid
					pos.y = pos.y + 32;
					pos.x = pos.x + 64;

					this.GlobalPosition = pos;
					//Improve enemy stats
					int rndHImprove = rnd.Next(0, 3);
					int rndDImprove = rnd.Next(0, 3);
					int rndAImprove = rnd.Next(0, 2);
					hp += rndHImprove;
					def += rndDImprove;
					agi += rndAImprove;
					EmitSignal(nameof(enemyKilled));
				}
			}
			else
			{
				EmitSignal(nameof(changeText), combatLog, "Combat Log: \n Player Miss!");
			}
		}
		else
		{
			EmitSignal(nameof(changeText), combatLog, "Combat Log: \n Player Miss!");
		}
	}
}
