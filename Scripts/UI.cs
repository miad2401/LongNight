using Godot;
using System;

public class UI : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public void setText(Label toChange, String text) {
		toChange.Text = text;
		GD.Print("set text reached");
	}
}
