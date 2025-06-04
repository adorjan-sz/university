using Godot;
using System;

public partial class GameVariables : Node
{

	public static GameVariables Instance { get; private set; }

	[Export] public int StartingMoney = 500;
	[Export] public int StartingTicketPrice = 20;
	[Signal] public delegate void MoneyChangedEventHandler(int newAmount);

	private int _currentMoney;
	private int _ticketPrice;
	private string _parkName;
	private bool _isGameOver = false;
    public override void _Ready()
	{
		if (Instance == null)
			Instance = this;
		else
		{
			QueueFree();
			return;
		}

		_currentMoney = StartingMoney;
		_ticketPrice = StartingTicketPrice;
	}
	public bool IsGameOver
    {
        get { return _isGameOver; }
        set { _isGameOver = value; }
    }
	public void NewGame()
	{
        _currentMoney = StartingMoney;
        _ticketPrice = StartingTicketPrice;
        _isGameOver = false;
        Engine.TimeScale = 1f;
    }
	public void ContinueGame()
	{
        _isGameOver = false;
        Engine.TimeScale = 1f;
    }
    public int GetMoney()
	{
		return _currentMoney;
	}
 	public void SetMoney(int amount)
    {
        _currentMoney = amount;
        EmitSignal(SignalName.MoneyChanged, _currentMoney);
    }
	public bool HasEnoughMoney(int amount)
	{
		return _currentMoney >= amount;
	}

	public bool DecreaseMoney(int amount)
	{
		if (HasEnoughMoney(amount))
		{
			_currentMoney -= amount;
			EmitSignal(SignalName.MoneyChanged, _currentMoney);
			return true;
		}
		return false;
	}

	public void AddMoney(int amount)
	{
		_currentMoney += amount;
		EmitSignal(SignalName.MoneyChanged, _currentMoney);
	}

	public int GetTicketPrice()
	{
		return _ticketPrice;
	}

	public void SetTicketPrice(int price)
	{
		_ticketPrice = price;
	}

	public void BuyTicket()
	{
		AddMoney(GetTicketPrice());
	}

	/// <summary>
	/// Gets or sets the safari park name.
	/// </summary>
	public string ParkName
	{
		get { return _parkName; }
		set { _parkName = value; }
	}
	public int TicketPrice
    {
        get { return _ticketPrice; }
        set { _ticketPrice = value; }
    }

}