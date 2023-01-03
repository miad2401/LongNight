using Godot;
using System;

public class Char : Node2D
{
	[Signal] public delegate void TurnHandler(bool hAction);
	
	public bool hasAction = true;
	[Export] public int numOfActions = 2;
	
	private Node2D enemy;
	private bool canMove = true;
	Vector2 enemyPos;

	private int hp;
	private int def;
	private int pow;

	Texture B11;
	Texture B12;
	Texture B13;
	Texture H11;
	Texture H12;
	Texture H13;
	Texture L11;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Connect to turn handler
		Connect(nameof(TurnHandler), GetNode("/root/Scene"), "turnHandler");

		//Load all character textures
		B11 = GD.Load<Texture>("res://Resources/Characters/Body/B1-1.png");
		B12 = GD.Load<Texture>("res://Resources/Characters/Body/B1-2.png");
		H11 = GD.Load<Texture>("res://Resources/Characters/Heads/H1-1.png");
		H12 = GD.Load<Texture>("res://Resources/Characters/Heads/H1-2.png");
		L11 = GD.Load<Texture>("res://Resources/Characters/Legs/L1-1.png");

		//Set basic stats
		hp = 10;
		def = 5;
		pow = 5;

		//Generate character
		generateChar();
	}
	
	//Handles all the input for the character
	public override void _Input(InputEvent inputEvent){
		enemy = GetNode<Node2D>("/root/Scene/Enemy");
		enemyPos = enemy.GlobalPosition;
		if (hasAction && Input.IsActionJustReleased("move_down")){
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
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{   
		
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
		int headChoice = rnd.Next(1, 3);//change to 10 when all sprites available
		int bodyChoice = rnd.Next(1, 3);
		int legsChoice = rnd.Next(1, 3);
		
		switch(headChoice){
			case 1:
				head.Texture = H11;
				break;
			case 2:
				head.Texture = H12;
				break;
		}
		
		switch(bodyChoice){
			case 1:
				body.Texture = B11;
				break;				
			case 2:
				body.Texture = B12;
				break;
		}
		
		switch(legsChoice){
			case 1:
				legs.Texture = L11;
				break;
		}
	}
}
