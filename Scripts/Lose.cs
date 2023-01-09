using Godot;
using System;

public class Lose : Control
{
	public void _on_Button_pressed()
	{
		GetTree().ChangeScene("res://Scenes/Menu.tscn");
	}
	
	public void setText(int killCount){
		Label lose = GetNode<Label>("/root/Control/Label");
		lose.Text = "You Lose\nKill count: " + killCount;
	}
}



