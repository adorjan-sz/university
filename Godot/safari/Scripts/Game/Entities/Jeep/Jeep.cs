using Godot;
using Safari.Scripts.Game.Entities;
using Safari.Scripts.Game.Road;
using Safari.Scripts.Game.Tiles;
using System;
using System.Collections.Generic;

public partial class Jeep : Node, Buyable
{
	[Export] public Control Control;
	[Export] public AnimatedSprite2D Sprite;
	[Export] public Label Label;
	[Export] public Area2D VissionArea;
	[Export] public float Speed = 150;
	[Export] public int Capacity = 4;

	private JeepParkingSlot _parkingSlot;
	private Vector2I _position;
	public Vector2I Position => _position;
	private enum State { Idle, Departing, Touring, Returning, Parking }
	private State _state;
	public string StateString => _state.ToString();
	private int _price;
	private List<Tourist> _passengers;
	private MapManager _mapManager;
	private Vector2I _nextCell;
	public Vector2I NextCell => _nextCell;

	int Buyable.Price { get => _price; }
	public bool Available { get => _state == State.Idle; }
	public int PassengerCount => _passengers.Count;

	public Jeep()
	{
		_price = 100;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	/// <summary>
	/// Sets up the jeep at the specified parking slot and initializes internal state.
	/// </summary
	public void Init(JeepParkingSlot slot, MapManager m)
	{
		_parkingSlot = slot;
		slot.IsOccupied = true;
		_mapManager = m;
		_position = slot.GridPosition;
		_nextCell = _position;
		Control.Position = MapToGlobal(_position);
		_state = State.Idle;
		_passengers = [];
	}


	/// <summary>
	/// Called each frame; drives movement logic based on current state.
	/// </summary>
	public override void _Process(double delta)
	{
		if (_state == State.Idle)
			return;

		// Convert nextCell to world coordinates
		Vector2 targetWorld = MapToGlobal(_nextCell);

		// Move towards targetWorld, clamped so we never overshoot
		float step = Speed * (float)delta;
		Control.Position = Control.Position.MoveToward(targetWorld, step);
		// Play correct animation
		UpdateAnimationDirection();

		// If arrived at target within a small threshold, snap and update state
		if (Control.Position.DistanceTo(targetWorld) < 0.1)
		{
			Control.Position = targetWorld;
			_position = _nextCell;

			switch (_state)
			{
				case State.Departing:
					// Departing: head to entrance cell
					if (_position == _mapManager.roadGraph.Entrance)
						StartTour();
					_nextCell = _mapManager.roadGraph.GetNextCellTowards(_position, _mapManager.roadGraph.Exit);
					if (_nextCell == _position)
						_nextCell = _mapManager.roadGraph.GetNearestRoadCell(_position);
					break;
				case State.Touring:
					// Touring: roam to exit, then unload and return
					if (_position == _mapManager.roadGraph.Exit)
						UnloadTourists();
					_nextCell = _mapManager.roadGraph.GetRandomNextCell(_position);
					break;
				case State.Returning:
					// Returning: back to entrance first
					if (_position == _mapManager.roadGraph.Entrance)
						_state = State.Parking;
					_nextCell = _mapManager.roadGraph.GetNextCellTowards(_position, _mapManager.roadGraph.Entrance);
					break;
				case State.Parking:
					// Parking: move into slot then idle
					if (_position == _parkingSlot.GridPosition)
						_state = State.Idle;
					if (_position == _parkingSlot.GridPosition + Vector2I.Right)
						_nextCell = _parkingSlot.GridPosition;
					else
						_nextCell = _mapManager.roadGraph.GetNextCellTowards(_position, _parkingSlot.GridPosition + Vector2I.Right);
					break;
				default:
					break;
			}
		}
	}

	// <summary>
	/// Loads tourists onto the jeep and begins the Departing state.
	/// </summary>
	public void LoadTourists(List<Tourist> tourists)
	{
		_state = State.Departing;
		foreach (var t in tourists)
		{
			if (_passengers.Count >= Capacity) break;
			_passengers.Add(t);
		}
	}

	/// <summary>
	/// Unloads all tourists and switches to Returning state.
	/// </summary>
	public void UnloadTourists()
	{
		_state = State.Returning;
		VissionArea.Monitoring = false;
		foreach (Tourist t in _passengers)
			t.LeaveReview();
		_passengers.Clear();
		Label.Text = $"0/{Capacity}";
	}

	private void StartTour()
	{
		_state = State.Touring;
		VissionArea.Monitoring = true;
		Label.Text = $"{_passengers.Count}/{Capacity}";
	}

	/// <summary>
	/// Chooses and plays the correct sprite animation based on movement direction.
	/// </summary>
	private void UpdateAnimationDirection()
	{
		Vector2 direction = (_nextCell - _position);
		string anim = direction switch
		{
			var a when a == Vector2I.Up => "Up",
			var a when a == Vector2I.Down => "Down",
			var a when a == Vector2I.Left => "Left",
			var a when a == Vector2I.Right => "Right",
			_ => "Down"
		};
		Sprite.Play(anim);
	}

	/// <summary>
	/// Called when a physics body enters the Jeep's detection area.
	/// Notifies each passenger tourist that they have seen the specified animal species.
	/// </summary>
	private void OnBodyEntered(Node body)
	{
		if (body is Animal animal)
		{
			foreach (Tourist t in _passengers)
				t.SeeAnimal(animal.AnimalsName);
		}
	}

	/// <summary>
	/// Converts map grid cell to world position.
	/// </summary>
	private Vector2 MapToGlobal(Vector2I pos)
	{
		return _mapManager.ToGlobal(_mapManager.Layer1.MapToLocal(pos));
	}

	/// <summary>
	/// Converts world coordinates back to map grid cell.
	/// </summary>
	private Vector2I GlobalToMap(Vector2 pos)
	{
		return _mapManager.Layer1.LocalToMap(_mapManager.ToLocal(pos));
	}
	public List<TouristSave> SaveTourist()
	{
		List<TouristSave> tourists = new List<TouristSave>();
		foreach (Tourist t in _passengers)
		{
			TouristSave ts = t.Save();

			tourists.Add(ts);
		}
		return tourists;
	}
	public JeepSave Save()
	{
		return new JeepSave(this);
	}
	public void Load(JeepSave save,MapManager mapManager, JeepParkingSlot slot)
	{
		_passengers = new List<Tourist>();
		foreach (TouristSave ts in save.Passengers)
		{
			Tourist t = new Tourist();
			t.Load(ts);
			_passengers.Add(t);
		}
		_position = new Vector2I((int)save.PosX, (int)save.PosY);
		_mapManager = mapManager;
		Control.Position = MapToGlobal(_position);
		_state = (State)Enum.Parse(typeof(State), save.State);
		_nextCell = new Vector2I(save.NextCellX, save.NextCellY);
		
		_parkingSlot = slot;
		slot.IsOccupied = true;
	}
}
