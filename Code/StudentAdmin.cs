using Godot;
using System;
using Godot.Collections;

public partial class StudentAdmin : VBoxContainer
{
	[Signal] public delegate void StudentNameSubmittedEventHandler(string Name);
	
	private AnimationPlayer AmountAP;
	private Label StudentName;
	private Label StudentTotal;
	private LineEdit TempName;
	private LineEdit TotalInput;
	
	private bool IsDeposit = true;
	private int TotalInt;
	private int TotalLimit = 1000;
	private Color RetryColor = new(1.0f, 0.03921568766236f, 0.12156862765551f);
	private Color DefaultColor = new(0.21366858482361f, 0.19990235567093f, 1.0f);
	
	public override void _Ready()
	{
		StudentName = (Label)GetNode("MainAcc/VBoxContainer/Name");
		StudentTotal = (Label) GetNode("MainAcc/VBoxContainer/Total");
		TempName = (LineEdit) GetNode("MainAcc/VBoxContainer/TempName");

		AmountAP = (AnimationPlayer) GetNode("Amount/AmountAP");
		TotalInput = (LineEdit) GetNode("Amount");
		TotalInput.Hide();
		StudentName.Visible = false;
		TempName.Visible = true;
	}
	public override Array<Dictionary> _GetPropertyList()
	{
		Dictionary Property = new()
		{
			{StudentName.Text, StudentTotal.Text}
		};
		
		var Parent = base._GetPropertyList();
		Parent.Add(Property);
		
		return base._GetPropertyList();
	}

	public void DepositPressed()
	{
		PrepareInput(StudentTotal);
	}

	public void WithdrawalPressed()
	{
		PrepareInput(StudentTotal);
		IsDeposit = false;
	}

	public void PrepareInput(Label st, bool isRetry = false)
	{
		TotalInput.Text = String.Empty;
		
		if (isRetry)
		{
			TotalInput.Modulate = RetryColor;
		}
		else
		{
			TotalInput.Modulate = DefaultColor;
		}
		
		TotalInput.Show();
		AmountAP.Play("Fade");
		
		if (Int32.TryParse(st.Text, out TotalInt))
		{
			GD.Print($"Student Total {TotalInt.ToString()} Assigned As Int!");
		}

		else
		{
			GD.Print($"Student Total Assignment: {st.Text} Failed! DELETING CORRUPTED STUDENT!");
			QueueFree();
		}
	}
	
	private void ChangeAmount(string AmountStr)
	{
		int AmountInt = 0;
		bool isParsableInt = Int32.TryParse(AmountStr, out AmountInt);
		
		AmountAP.PlayBackwards("Fade");

		if (isParsableInt)
		{
			if (TotalInt < 0)
			{
				AmountInt *= -1;
			}
			if (IsDeposit)
			{
				TotalInt += AmountInt;
			}
			else
			{
				TotalInt -= AmountInt;
			}
		}
		
		else
		{
			GD.PushWarning($"Value: {AmountStr} is not a numeric value");
			PrepareInput(StudentTotal, true);
		}

		StudentTotal.Text = TotalInt.ToString();
	}

	public void DeletePressed()
	{
		QueueFree();
	}

	public void ProcessStudentData(Dictionary<string, string> data)
	{
		foreach (var Key in data.Keys)
		{
			StudentName.Text = Key;
			StudentTotal.Text = data[Key];
		}
	}

	public Dictionary<string, string> GetStudentData()
	{
		Dictionary<string, string> Data = new Dictionary<string, string>();
		Data.Add(StudentName.Text, StudentTotal.Text);
		return Data;
	}

	public void ChangeName(string newName)
	{
		StudentName.Text = newName;
	}

	public void TempNameChanged(string Name)
	{
		EmitSignal(SignalName.StudentNameSubmitted, Name);
		TempName.Visible = false;
		ChangeName(Name);
		StudentName.Visible = true;
	}
	
}
