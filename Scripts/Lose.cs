using Godot;
using System;

public class Lose : Control
{
	public void _on_Button_pressed()
	{
		GetTree().ChangeScene("res://Scenes/Menu.tscn");
	}
}



