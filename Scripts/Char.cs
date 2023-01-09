using Godot;
using System;

public class Char : Node2D
{
	[Signal] public delegate void TurnHandler(bool hAction);
	[Signal] public delegate void changeText(Node toChange, String text);
	[Signal] public delegate void attackEnemy(int roll);
	//[Signal] public delegate void changeLoseText(int amount);

	public bool hasAction = true;
	[Export] public int numOfActions = 2;
	
	private Node2D enemy;
	private bool canMove = true;
	Vector2 enemyPos;
	public bool mode = false; //false = normal, true = infinite

	[Export] private int hp = 10;
	[Export] private int def = 3;
	[Export] private int agi = 2;

	PackedScene Lose;
	PackedScene Win;

	Texture B11;
	Texture B12;
	Texture B13;
	Texture H11;
	Texture H12;
	Texture H13;
	Texture L11;
	Texture L12;
	Texture L13;

	Texture B21;
	Texture B22;
	Texture B23;
	Texture H21;
	Texture H22;
	Texture H23;
	Texture L21;
	Texture L22;
	Texture L23;

	Texture B31;
	Texture B32;
	Texture B33;
	Texture H31;
	Texture H32;
	Texture H33;
	Texture L31;
	Texture L32;
	Texture L33;

	int killCount;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Connect signals
		Connect(nameof(TurnHandler), GetNode("/root/Scene"), "turnHandler");
		Connect(nameof(changeText), GetNode("/root/Scene/Char/Camera2D/Control"), "setText");
		Connect(nameof(attackEnemy), GetNode("/root/Scene/Enemy"), "takeDamage");
		//Connect(nameof(changeLoseText), GetNode("/root/Lose"), "setText");

		//Load all character textures
		B11 = GD.Load<Texture>("res://Resources/Characters/Body/B1-1.png");
		B12 = GD.Load<Texture>("res://Resources/Characters/Body/B1-2.png");
		B13 = GD.Load<Texture>("res://Resources/Characters/Body/B1-3.png");
		H11 = GD.Load<Texture>("res://Resources/Characters/Heads/A1-1.png");
		H12 = GD.Load<Texture>("res://Resources/Characters/Heads/A1-2.png");
		H13 = GD.Load<Texture>("res://Resources/Characters/Heads/A1-3.png");
		L11 = GD.Load<Texture>("res://Resources/Characters/Legs/C1-1.png");
		L12 = GD.Load<Texture>("res://Resources/Characters/Legs/C1-2.png");
		L13 = GD.Load<Texture>("res://Resources/Characters/Legs/C1-3.png");

		B21 = GD.Load<Texture>("res://Resources/Characters/Body/B2-1.png");
		B22 = GD.Load<Texture>("res://Resources/Characters/Body/B2-2.png");
		B23 = GD.Load<Texture>("res://Resources/Characters/Body/B2-3.png");
		H21 = GD.Load<Texture>("res://Resources/Characters/Heads/A2-1.png");
		H22 = GD.Load<Texture>("res://Resources/Characters/Heads/A2-2.png");
		H23 = GD.Load<Texture>("res://Resources/Characters/Heads/A2-3.png");
		L21 = GD.Load<Texture>("res://Resources/Characters/Legs/C2-1.png");
		L22 = GD.Load<Texture>("res://Resources/Characters/Legs/C2-2.png");
		L23 = GD.Load<Texture>("res://Resources/Characters/Legs/C2-3.png");

		B31 = GD.Load<Texture>("res://Resources/Characters/Body/B3-1.png");
		B32 = GD.Load<Texture>("res://Resources/Characters/Body/B3-2.png");
		B33 = GD.Load<Texture>("res://Resources/Characters/Body/B3-3.png");
		H31 = GD.Load<Texture>("res://Resources/Characters/Heads/A3-1.png");
		H32 = GD.Load<Texture>("res://Resources/Characters/Heads/A3-2.png");
		H33 = GD.Load<Texture>("res://Resources/Characters/Heads/A3-3.png");
		L31 = GD.Load<Texture>("res://Resources/Characters/Legs/C3-1.png");
		L32 = GD.Load<Texture>("res://Resources/Characters/Legs/C3-2.png");
		L33 = GD.Load<Texture>("res://Resources/Characters/Legs/C3-3.png");

		//Lose = GD.Load<PackedScene>("res://Resources/Scenes/Lose.tscn");
		//Win = GD.Load<PackedScene>("res://Resources/Scenes/Win.tscn");
		killCount = 0;

		//Generate character
		generateChar();
	}
	
	//Handles all the input for the character
	//TODO: Add attack and defend actions
	//TODO: handle multiple enemies
	public override void _Input(InputEvent inputEvent){
		enemy = GetNode<Node2D>("/root/Scene/Enemy");
		enemyPos = enemy.GlobalPosition;
		Sprite attackSprt = GetNode<Sprite>("/root/Scene/Char/Attack/Attack");
		if (hasAction && Input.IsActionJustReleased("move_down")){
			attackSprt.Texture = GD.Load<Texture>("res://Resources/Characters/AttackSlash-RightDown.png");
			Vector2 pos = this.GlobalPosition;
			if (!(enemyPos.x == pos.x + 64) || !(enemyPos.y == pos.y + 32)){
				pos.x = pos.x + 64;
				pos.y = pos.y + 32;
				this.GlobalPosition = pos;
				numOfActions--;
				if (numOfActions == 0)
				{
					hasAction = false;
					EmitSignal(nameof(TurnHandler), hasAction);
				}
			} else {
				GD.Print("Enemy pos:", enemyPos);
				GD.Print("Player pos:", pos);
				GD.Print("Cannot move down");
			}
		}
		if (hasAction && Input.IsActionJustReleased("move_up"))
		{
			Vector2 pos = this.GlobalPosition;
			attackSprt.Texture = GD.Load<Texture>("res://Resources/Characters/AttackSlash-LeftUp.png");
			if (!(enemyPos.x == pos.x - 64) || !(enemyPos.y == pos.y - 32)){
				pos.x = pos.x - 64;
				pos.y = pos.y - 32;
				this.GlobalPosition = pos;
				numOfActions--;
				if (numOfActions == 0)
				{
					hasAction = false;
					EmitSignal(nameof(TurnHandler), hasAction);
				}
			} else {
				GD.Print("Enemy pos:", enemyPos);
				GD.Print("Player pos:", pos);
				GD.Print("Cannot move up");
			}
		}
		if (hasAction && Input.IsActionJustReleased("move_right"))
		{
			Vector2 pos = this.GlobalPosition;
			attackSprt.Texture = GD.Load<Texture>("res://Resources/Characters/AttackSlash-RightDown.png");
			if (!(enemyPos.x == pos.x + 64) || !(enemyPos.y == pos.y - 32)){
				pos.x = pos.x + 64;
				pos.y = pos.y - 32;
				this.GlobalPosition = pos;
				numOfActions--;
				if (numOfActions == 0)
				{
					hasAction = false;
					EmitSignal(nameof(TurnHandler), hasAction);
				}
			} else {
				GD.Print("Enemy pos:", enemyPos);
				GD.Print("Player pos:", pos);
				GD.Print("Cannot move right");
			}
		}
		if (hasAction && Input.IsActionJustReleased("move_left"))
		{
			Vector2 pos = this.GlobalPosition;
			attackSprt.Texture = GD.Load<Texture>("res://Resources/Characters/AttackSlash-LeftUp.png");
			if (!(enemyPos.x == pos.x - 64) || !(enemyPos.y == pos.y + 32)){
				pos.x = pos.x - 64;
				pos.y = pos.y + 32;
				this.GlobalPosition = pos;
				numOfActions--;
				if (numOfActions == 0)
				{
					hasAction = false;
					EmitSignal(nameof(TurnHandler), hasAction);
				}
			} else {
				GD.Print("Enemy pos:", enemyPos);
				GD.Print("Player pos:", pos);
				GD.Print("Cannot move left");
			}
		}

		if (hasAction && Input.IsActionJustReleased("player_attack"))
		{
			//Get position of character, get map, get position of mouse
			Vector2 pos = this.GlobalPosition;
			TileMap map = GetNode<TileMap>("/root/Scene/Base");
			Vector2 mousePos = GetGlobalMousePosition();
			Vector2 mousePosLocal = map.ToLocal(mousePos);
			Vector2 playerPosLocal = map.ToLocal(pos);
			Vector2 enemyPosLocal = map.ToLocal(enemyPos);

			//Get Tilemap coords of all positions
			Vector2 mouseTilePos = map.WorldToMap(mousePosLocal);
			Vector2 playerTilePos = map.WorldToMap(playerPosLocal);
			Vector2 enemyTilePos = map.WorldToMap(enemyPosLocal);

			GD.Print("Mouse pos: " + mousePos + " | Mouse Cell: " + mouseTilePos);
			GD.Print("Player pos: " + pos + " | Player Cell: " + playerTilePos);
			GD.Print("Enemy pos: " + enemyPos + " | Enemy Cell: " + enemyTilePos);
			//Check if the tile clicked is the tile the enemy is on and check if player is beside enemy
			if (mouseTilePos == enemyTilePos && (enemyTilePos.x == playerTilePos.x -1 || enemyTilePos.x == playerTilePos.x + 1 || enemyTilePos.y == playerTilePos.y - 1 || enemyTilePos.y == playerTilePos.y + 1))
			{
				Attack();
				numOfActions--;
				if (numOfActions == 0)
				{
					hasAction = false;
					EmitSignal(nameof(TurnHandler), hasAction);
				}
				else
				{
					GD.Print("Enemy pos:", enemyPos);
					GD.Print("Player pos:", pos);
					GD.Print("Cannot attack tile");
				}
			}
		}
	}

	public void Attack()
	{
		AnimationPlayer animPlayer = GetNode<AnimationPlayer>("/root/Scene/Char/Attack/AnimationPlayer");
		Animation attackAnim = GD.Load<Animation>("res://Attack.tres");
		animPlayer.Play(attackAnim.ResourceName);
		Random rnd = new Random();
		int roll = rnd.Next(1, 11);
		GD.Print("Player Attack Roll: " + roll);
		


		EmitSignal(nameof(attackEnemy), roll);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta){   
		
	}
	
	//Tells the player that its their turn
	public void onEnemyTurnOver(){
		hasAction = true;
		numOfActions = 2;
	}
	
	//Generate the character and stats
	public void generateChar(){
		Sprite head = GetNode<Sprite>("/root/Scene/Char/Head/Head");
		Sprite body = GetNode<Sprite>("/root/Scene/Char/Body/Body");
		Sprite legs = GetNode<Sprite>("/root/Scene/Char/Legs/Legs");
		
		Random rnd = new Random();
		int headChoice = rnd.Next(1, 10);//change to 10 when all sprites available
		int legsChoice = rnd.Next(1, 4);
		
		switch(headChoice){
			case 1:
				head.Texture = H11;
				body.Texture = B11;
				def -= 1;
				hp += 2;
				break;
			case 2:
				head.Texture = H12;
				body.Texture = B12;
				def += 1;
				hp += 1;
				break;
			case 3:
				head.Texture = H13;
				body.Texture = B13;
				def -= 1;
				hp += 1;
				break;
			case 4:
				head.Texture = H21;
				body.Texture = B21;
				def += 1;
				hp += 1;
				break;
			case 5:
				head.Texture = H22;
				body.Texture = B22;
				def += 1;
				hp += 2;
				break;
			case 6:
				head.Texture = H23;
				body.Texture = B23;
				def += 1;
				hp -= 1;
				break;
			case 7:
				head.Texture = H31;
				body.Texture = B31;
				def += 1;
				hp -= 2;
				break;
			case 8:
				head.Texture = H32;
				body.Texture = B32;
				def -= 1;
				hp -= 1;
				break;
			case 9:
				head.Texture = H33;
				body.Texture = B33;
				def += 1;
				hp -= 1;
				break;
		}
		
		switch(legsChoice){
			case 1:
				legs.Texture = L11;
				agi += 1;
				break;
			case 2:
				legs.Texture = L21;
				agi -= 1;
				break;
			case 3:
				legs.Texture = L31;
				agi += 2;
				break;
		}
		
		updateStats();
	}
	
	//Update stats
	public void updateStats(){
		Label stats = GetNode<Label>("/root/Scene/Char/Camera2D/Control/TopBar/Stats");
		String statsText = "==========Stats===========\nHP: " + hp + "\nDEF: " + def + "\nAGI: " + agi;
		EmitSignal(nameof(changeText), stats, statsText);
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
				EmitSignal(nameof(changeText), combatLog, "Combat Log: \n Enemy Hit!");
				hp -= amount;
				if (hp <= 0)
				{
					GetTree().ChangeSceneTo(GD.Load<PackedScene>("res://Scenes/Lose.tscn"));
					//EmitSignal(nameof(changeLoseText), "You Lose\nKill count: " + killCount);
				}
			} else {
				EmitSignal(nameof(changeText), combatLog, "Combat Log: \nEnemy Miss!");
			}
		} else {
			EmitSignal(nameof(changeText), combatLog, "Combat Log: \nEnemy Miss!");
		}
		updateStats();
	}

	public void updateKillCount()
	{
		Label killCounter = GetNode<Label>("/root/Scene/Char/Camera2D/Control/TopBar/KillCount");
		killCount++;
		EmitSignal(nameof(changeText), killCounter, "Kill count: " + killCount);
		//if (!mode)
		//{
		//	if (killCount == 4)
		//	{
		//		GetTree().ChangeSceneTo(GD.Load<PackedScene>("res://Scenes/Win.tscn"));
		//	}
		//}
	}
}
