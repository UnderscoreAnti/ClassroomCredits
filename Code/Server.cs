using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

public partial class Server : Node
{
	[Export(PropertyHint.File)] public string DebugSaveFilePath;

	private string SaveFilePath = "user://ClassData.json";

	private string CurrentClassName;
	private Array<Dictionary<string, string>> CurrentClass;
	private Dictionary<string, Array<Dictionary<string, string>>> AllClassesData;

	private int ServerPort = 9999;
	private int MaxConnections = 50;

	public override void _EnterTree()
	{
		// Guarantees a save file is present at run time
		WriteDataToFile();
	}
	
	public override void _Ready()
	{
		Debug();
	}

	public void InitializeNetwork()
	{
		ENetMultiplayerPeer Peer = new();

		Multiplayer.PeerConnected += ClientConnected;
		Multiplayer.PeerDisconnected += ClientDisconnected;

		Peer.CreateServer(ServerPort, MaxConnections);

		Multiplayer.MultiplayerPeer = Peer;
	}


	public void ClientConnected(long ClientID)
	{
		GD.Print($"{ClientID.ToString()} Connected to the server.");
		RpcId(ClientID, "GetCurrentClassData", CurrentClass);
	}

	public void ClientDisconnected(long ClientID)
	{
		GD.Print($"{ClientID.ToString()} Disconnected from the server.");
	}

	public void SaveStudentData()
	{
		AllClassesData[CurrentClassName] = CurrentClass;
	}

	public void ChangeClassName(string NewName)
	{
		AllClassesData.Remove(CurrentClassName);
		AllClassesData.Add(NewName, CurrentClass);
	}

	public void AddStudentToCurrentClass(string StudentName)
	{
		Dictionary<string, string> NewStudent = new() {{StudentName, "0000"}};
		CurrentClass.Add(NewStudent);
	}

	public void AddNewClassToClassData(string ClassName)
	{
		Array<Dictionary<string, string>> NewClass = new();
		AllClassesData.Add(ClassName, NewClass);
	}
	

	public void WriteDataToFile(Variant Data = new())
	{
		string Content = JSON.Stringify(Data);
		var File = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Write);
		File.StoreString(Content);
		
		File.Dispose();
	}

	public void SendCurrentClassData()
	{
		Rpc("GetCurrentClassFromServer", CurrentClass);
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void Debug()
	{
		if (!ServerActiveNotif.IsServerActive)
		{
			InitializeNetwork();
		}
		
		else if (!ServerActiveNotif.isAdminActive)
		{
			PackedScene PScene = (PackedScene) ResourceLoader.Load("res://Node/AdminPage.tscn");
			AdminPage PageLoader = (AdminPage) PScene.Instantiate();
			
			AddChild(PageLoader);
		}

		else
		{
			PackedScene PScene = (PackedScene) ResourceLoader.Load("res://Node/StudentPage.tscn");
			AdminPage PageLoader = (AdminPage) PScene.Instantiate();
			
			AddChild(PageLoader);
		}
	}
}
