using Godot;
using System;

public class Menu : Control
{
	public void _on_Start_pressed()
	{
		GetTree().ChangeScene("res://Scenes/Scene.tscn");
	}
	
	public void _on_Credits_pressed()
	{
		GetTree().ChangeScene("res://Scenes/Credits.tscn");
	}
	
	public void _on_Quit_pressed()
	{
		GetTree().Quit();
	}
}



