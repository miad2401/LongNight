using Godot;
using System;

public class Scene : Node2D
{
	Node2D player;
	public bool turn = false; //false = player turn, true = enemy turn
	// Called when the node enters the scene tree for the first time.
	[Signal] public delegate void enemyTurnOver();
	public override void _Ready()
	{
		Connect(nameof(enemyTurnOver), GetNode("/root/Scene/Char"), "onEnemyTurnOver");
		player = GetNode<Node2D>("/root/Scene/Char");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		player = GetNode<Node2D>("/root/Scene/Char");
	}
	
	public void turnHandler(bool hasAction){
		GD.Print("turn handler reached");
		if (hasAction == false){
			turn = true;
			enemyTurn();
			hasAction = true;
			EmitSignal(nameof(enemyTurnOver));
		}
	}
	
	//TODO: Implement enemy turn
	public void enemyTurn(){
		if (turn){
			GD.Print("enemy turn reached");
		}
		turn = false;
	}
}
