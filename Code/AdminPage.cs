using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;
	
public partial class AdminPage : Control
{
	[Export] public PackedScene StudentAdminScene;

	private HFlowContainer MainPage;
	private AdminSideMenu AdminMenu;
	
	public Godot.Collections.Array CurrentStudentList;
	public string CurrentClassName;

	private int ServerPort = 9999;
	private string ServerIP = "localhost";
	private int ServerID = 1;

	public override void _Ready()
	{
		MainPage = (HFlowContainer) GetNode("MainMenu/MainPage");
		AdminMenu = (AdminSideMenu) GetNode("MainMenu/AdminSideMenu");
		InitializeNetwork();
	}

	public void InitializeNetwork()
	{
		ENetMultiplayerPeer Peer = new();
		
		Peer.CreateClient(ServerIP, ServerPort);
		Multiplayer.MultiplayerPeer = Peer;
	}

	public void AddNewStudentToMainPage()
	{
		StudentAdmin InstanceClass = (StudentAdmin) StudentAdminScene.Instantiate();
		MainPage.AddChild(InstanceClass);
	}

	public void GetCurrentClassDataOld(Array<Godot.Collections.Dictionary<string, string>> Data)
	{
		GD.Print("Get ClassData RPC call received on Admin!");
		if(Data == new Array<Godot.Collections.Dictionary<string, string>>())
		{
			GD.Print("Empty class data received!");
			return;
		}

		foreach (Godot.Collections.Dictionary<string, string> student in Data)
		{
			StudentAdmin InstanceClass = (StudentAdmin) StudentAdminScene.Instantiate();
			InstanceClass.ProcessStudentData(student);
			AddChild(InstanceClass);
		}
	}

	public void GetSelectedClass(string SelectedClass)
	{
		RpcId(ServerID, nameof(GetCurrentClassData), SelectedClass);
	}

	public void NewClassToDataBaseRequest()
	{
		RpcId(ServerID, nameof(AddNewClassToDatabase));
	}

	public void SendAdminData()
	{
		RpcId(1, nameof(GetAdminData));
	}

	public void NewNameInputted(string NewName)
	{
		RpcId(ServerID, nameof(UpdateClassName), NewName);
	}

	public void UnlockClass()
	{
		Rpc(nameof(RequestClassUnlock));
	}

	[RPC]
	public void ProcessClassData(Array<Dictionary<string, string>> ClassData)
	{
		GetCurrentClassDataOld(ClassData);
	}

	[RPC]
	public void UpdateClassName(string NewName)
	{
		CurrentClassName = NewName;
		AdminMenu.ClassNameChangedOnServer(NewName);
	}
	
	[RPC]
	public void ConfirmServerConnection(string ServerMessage)
	{
		GD.Print($"{ServerMessage}");
		SendAdminData();
	}
	
	[RPC]
	public void OnStudentNameEntered(string NewStudentName)
	{
		RpcId(ServerID, nameof(OnStudentNameEntered), NewStudentName);
	}
	
	// GD BoilerPlate

	[RPC]
	public void GetAdminData()
	{
		//
	}
	
	[RPC]
	public void AddNewClassToDatabase()
	{
		//
	}
	
	[RPC]
	public void CurrentClassUpdate(string StudentName, bool isRemoval = false)
	{
		GD.Print("Update ClassData RPC call received on Admin!");
	}
	
	[RPC]
	public void GetCurrentClassData(string selectedClass) {  }


	[RPC]
	public void UpdateDataReqest(int RequestType)
	{
		//
	}
	
	[RPC]
	public void RequestClassUnlock(bool isUnlockRequest = true)
	{
		//
	}
	
	[RPC]
	public void ClassUnlocked(bool isUnlocked = false)
	{
		//
	}

	[RPC]
	public void RequestAddNewStudent(Dictionary<string, string> StudentData)
	{
		//
	}
}
