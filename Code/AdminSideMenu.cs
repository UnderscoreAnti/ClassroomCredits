using Godot;
using System;

public partial class AdminSideMenu : VBoxContainer
{
	[Signal] public delegate void NewStudentCalledEventHandler();
	[Signal] public delegate void NewClassCreatedEventHandler();
	[Signal] public delegate void ChangeDictKeyNameEventHandler(string newName);
	[Signal] public delegate void ClassSelectedEventHandler(string selectedClass);
	[Signal] public delegate void CurrentClassUnlockedEventHandler();
	
	[Export] public PackedScene ClassScene;

	private LineEdit ClassNameInput;
	private ClassAdmin CurrentClassAdmin;
	private AnimationPlayer NameInAnimP;

	public override void _Ready()
	{
		ClassNameInput = (LineEdit) GetNode("ClassNameInput");
		NameInAnimP = (AnimationPlayer) GetNode("ClassNameInput/NameInAnimP");
	}

	public void OpenMenuPressed()
	{
		//
	}

	public void NewClassPressed()
	{
		ClassAdmin InstanceClass = (ClassAdmin) ClassScene.Instantiate();
		InstanceClass.ClassSelected += ClassAdminSelected;
		InstanceClass.NewStudentAdded += CreateNewStudent;
		InstanceClass.CurrentClassUnlocked += UnlockCurrentClassNotif;
		ClassNameInput.Show();
		
		AddChild(InstanceClass);
		ClassAdminSelected(InstanceClass);
		EmitSignal(SignalName.NewClassCreated);
	}

	public void UnlockCurrentClassNotif()
	{
		EmitSignal(SignalName.CurrentClassUnlocked);
	}

	public void ClassAdminSelected(ClassAdmin SelectedClass)
	{
		ClassNameInput.PlaceholderText = SelectedClass.ClassName;
		CurrentClassAdmin = SelectedClass;

		EmitSignal(SignalName.ClassSelected, SelectedClass.ClassName);
	}

	public void ClassNameInputted(string NewName)
	{
		CurrentClassAdmin.ClassNameChanged(NewName);
		ClassNameInput.Clear();
		EmitSignal(SignalName.ChangeDictKeyName, NewName);
	}

	public void ClassNameChangedOnServer(string NewName)
	{
		CurrentClassAdmin.ClassNameChanged(NewName);
	}

	public void CreateNewStudent()
	{
		EmitSignal(SignalName.NewStudentCalled);
	}
}
