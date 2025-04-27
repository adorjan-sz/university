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
}