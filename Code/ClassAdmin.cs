using Godot;
using System;

public partial class ClassAdmin : HBoxContainer
{
	[Signal] public delegate void ClassSelectedEventHandler(ClassAdmin SelectedClass);
	[Signal] public delegate void NewStudentAddedEventHandler();
	[Signal] public delegate void CurrentClassUnlockedEventHandler();
	
	
	private Button SelectClass;
	private Button Delete;

	public String ClassName;
	
	public override void _Ready()
	{
		SelectClass = (Button) GetNode("SelectClass");
	}

	public void ClassNameChanged(string NewName)
	{
		ClassName = NewName;
		SelectClass.Text = NewName;
	}

	private void DeletePressed()
	{
		QueueFree();
	}

	private void NewStudentPressed()
	{
		EmitSignal(SignalName.NewStudentAdded);
	}

	public void UnlockClassPressed()
	{
		EmitSignal(SignalName.ClassSelected, this);
		EmitSignal(SignalName.CurrentClassUnlocked);
	}
	

	private void SelectClassPressed()
	{
		EmitSignal(SignalName.ClassSelected, this);
	}
	
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
