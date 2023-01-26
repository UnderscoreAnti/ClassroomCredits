using Godot;
using System;
using Godot.Collections;

public partial class Student : HBoxContainer
{
	private Label StudentName;
	private Label StudentTotal;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ProcessStudentData(Dictionary<string, string> Data)
	{
		foreach (string Key in Data.Keys)
		{
			StudentName.Text = Key;
			StudentTotal.Text = Data[Key];
		}
	}
}
