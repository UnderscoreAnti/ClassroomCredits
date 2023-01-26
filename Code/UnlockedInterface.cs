using Godot;
using System;

public partial class UnlockedInterface : MarginContainer
{
	[Signal] public delegate void StudentNameEnteredEventHandler(string StudentName);
	
	private LineEdit StudentNameInput;
	private Button AddStudentButton;

	public override void _Ready()
	{
		StudentNameInput = (LineEdit) GetNode("LineEdit");
		AddStudentButton = (Button) GetNode("AddStudentButton");
		
		StudentNameInput.Visible = false;
	}

	public void OnAddStudentButtonPressed()
	{
		StudentNameInput.Visible = !StudentNameInput.Visible;
		AddStudentButton.Visible = false;
	}

	public void OnNameEntered(string EnteredName)
	{
		EmitSignal(SignalName.StudentNameEntered, EnteredName);
		StudentNameInput.Visible = !StudentNameInput.Visible;
	}
}
