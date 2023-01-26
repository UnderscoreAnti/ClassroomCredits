using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Godot.Collections;
using FileAccess = Godot.FileAccess;

public partial class DemoNode : Node
{
	[Export] public string FileName = "JSONSaveFile.json";
	[Export] public PackedScene ObjectData;
	private StudentAdmin Stud;


	public override void _Ready()
	{
		Stud = (StudentAdmin) GetNode("StudentAdmin");
		GD.Print("Student Assigned!");
		var saveData = Stud.GetStudentData();
		Save(saveData);
		
		Stud.QueueFree();
		GD.Print("Old Student Killed! :O");
		// var NewStud = (StudentAdmin) Load();
		// AddChild(NewStud);
		GD.Print("Oh lord I'm stretching this!");
		// NewStud.ChangeName("Lupita Nyong'o");
		// GD.Print("Done!!!!");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//
	}

	public void Save(object obj)
	{
		string jstring = JsonSerializer.Serialize(obj);
		GD.Print(jstring);
		File.WriteAllText(FileName, jstring);
	}

	public List<SortedDictionary<string, int>> Load()
	{
		FileStream fs = File.Create(FileName);

		return JsonSerializer.Deserialize<List<SortedDictionary<string, int>>>(fs);
	}
}
