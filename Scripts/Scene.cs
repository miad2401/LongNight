using Godot;
using System;
using System.Threading;

public class Scene : Node2D
{
	Node2D player;
	public bool turn = false; //false = player turn, true = enemy turn

	AudioStreamPlayer audio;

	[Signal] public delegate void enemyTurnOver();
	[Signal] public delegate void enemyTurnHandler();
	[Signal] public delegate void changeText(Node toChange, String text);

	public override void _Ready()
	{
		Connect(nameof(enemyTurnOver), GetNode("/root/Scene/Char"), "onEnemyTurnOver");
		Connect(nameof(enemyTurnHandler), GetNode("/root/Scene/Enemy"), "turn");
		Connect(nameof(changeText), GetNode("/root/Scene/Char/Camera2D/Control"), "setText");
		player = GetNode<Node2D>("/root/Scene/Char");
		audio = GetNode<AudioStreamPlayer>("/root/Scene/AudioStreamPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		player = GetNode<Node2D>("/root/Scene/Char");
		if (!audio.Playing)
		{
			audio.Play();
		}
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
	
	public void enemyTurn(){
		if (turn){
			GD.Print("enemy turn reached");
			EmitSignal(nameof(enemyTurnHandler));
		}
		turn = false;
	}
}
