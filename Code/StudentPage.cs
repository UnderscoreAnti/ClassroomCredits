using Godot;
using System;
using Godot.Collections;

public partial class StudentPage : Control
{
	[Export] private PackedScene StudentContainerScene;
	[Export] private PackedScene UnlockedClassScene;

	private HFlowContainer MainPage;
	private MarginContainer UnlockedInterface;

	private Godot.Collections.Array CurrentStudentList;
	private string CurrentClassName;

	private int ServerID = 1;
	private int ServerPort = 9999;
	private string ServerIP = "localhost";
	
	public override void _Ready()
	{
		MainPage = (HFlowContainer) GetNode("MainPage");
		UnlockedInterface = (MarginContainer) GetNode("UnlockedInterface");
		InitializeNetwork();
	}
	
	public void InitializeNetwork()
	{
		ENetMultiplayerPeer Peer = new();
		Peer.CreateClient(ServerIP, ServerPort);
		Multiplayer.MultiplayerPeer = Peer;
	}

	public void AddNewStudentToMainPage(Godot.Collections.Dictionary<string, string> StudentData)
	{
		Student InstanceClass = (Student) StudentContainerScene.Instantiate();
		
		if (StudentData != new Godot.Collections.Dictionary<string, string>())
		{
			InstanceClass.ProcessStudentData(StudentData); 
		}
		
		MainPage.AddChild(InstanceClass);
	}

	[Rpc]
	public void OnStudentNameEntered(string NewStudentName)
	{
		RpcId(ServerID, nameof(OnStudentNameEntered), NewStudentName);
	}
	

	[Rpc]
	public void ProcessClassData(Array<Dictionary<string, string>> ClassData) { }
	
	[Rpc]
	public void UpdateClassName(string ClassName) { }
	
	// GD Boilerplate
	
	[Rpc]
	public void GetAdminData()
	{
		//
	}
	
	[Rpc]
	public void GetCurrentClassData(string selectedClass)
	{
		GD.Print("ClassData RPC call received on Student!");
	}

	[Rpc]
	public void CurrentClassUpdate(string StudentName, bool isRemoval = false)
	{
		GD.Print("Update ClassData RPC call received on Student!");
	}

	[Rpc]
	public void ClassUnlocked(bool isUnlocked = false)
	{
		
	}

	[Rpc]
	public void RequestAddNewStudent(Dictionary<string, string> StudentData)
	{
		GD.Print("Add New Student RPC call sent on Student!");
	}

	[Rpc]
	public void ConfirmServerConnection(string ServerMessage)
	{
		GD.Print($"{ServerMessage}");
	}
	
	[Rpc]
	public void RequestClassUnlock(bool isUnlockRequest = true)
	{
		//
	}

	[Rpc]
	public void UpdateDataReqest(int RequestType)
	{
		//
	}
	
	[Rpc]
	public void AddNewClassToDatabase()
	{
		//
	}
	


}
