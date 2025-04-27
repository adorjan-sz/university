using Godot;
using Safari.Scripts.Game.Tiles;
using System;

public partial class BuyMenuOverlay : TabContainer
{
	[Export] public Texture2D EnvironmentIcon;
	[Export] public Texture2D AnimalsIcon;
	[Export] public Texture2D TransportIcon;
	[Export] public Texture2D MouseIcon;

	[Export] public Button FlowerButton;
	[Export] public Button LogButton;
	[Export] public Button RockButton;
	[Export] public Button WaterButton;
	[Export] public Button TreeButton;
	[Export] public Button RoadButton;
	[Export] public Button JeepButton;
	[Export] public Button SellButton;
	[Export] public Button DragButton;

	[Signal] public delegate void PlaceTileEventHandler(Tile type);
	[Signal] public delegate void SelectedTileChangedEventHandler(Tile type);
	[Signal] public delegate void CameraDragEventHandler(bool enabled);
	[Signal] public delegate void JeepBoughtEventHandler();

	private Tile selectedTile;
	private bool cameraDragEnabled;

	private enum Mode {Tile, Animal, Jeep };
	private Mode mode;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetTabTitle(0, "");
		SetTabIcon(0, EnvironmentIcon);

		SetTabTitle(1, "");
		SetTabIcon(1, AnimalsIcon);

		SetTabTitle(2, "");
		SetTabIcon(2, TransportIcon);

		SetTabTitle(3, "");
		SetTabIcon(3, MouseIcon);

		FlowerButton.Pressed += () => SelectTileType(new FlowerTile(true));
		LogButton.Pressed += () => SelectTileType(new LogTile(true));
		RockButton.Pressed += () => SelectTileType(new RockTile(true));
		WaterButton.Pressed += () => SelectTileType(new WaterTile());
		TreeButton.Pressed += () => SelectTileType(new TreeTile(TreeVariant.Full, true));
		RoadButton.Pressed += () => SelectTileType(new RoadTile());
		SellButton.Pressed += () => SelectTileType(new EmptyTile());
		DragButton.Pressed += () => ChangeDragState(true);
		JeepButton.Pressed += OnJeepButtonPressed;

		SetTileTooltip(FlowerButton, new FlowerTile(true));
		SetTileTooltip(LogButton, new LogTile(true));
		SetTileTooltip(RockButton, new RockTile(true));
		SetTileTooltip(WaterButton, new WaterTile());
		SetTileTooltip(TreeButton, new TreeTile(TreeVariant.Full, true));
		SetTileTooltip(RoadButton, new RoadTile());
		ChangeDragState(true);
	}

	private void OnJeepButtonPressed()
	{
		mode = Mode.Jeep;
		ChangeDragState(false);
		EmitSignal(SignalName.SelectedTileChanged, new EmptyTile());
	}

	private void ChangeDragState(bool enabled)
	{
		cameraDragEnabled = enabled;
		if (cameraDragEnabled)
		{
			Input.SetDefaultCursorShape(Input.CursorShape.Drag);
			EmitSignal(SignalName.SelectedTileChanged, new EmptyTile());
			selectedTile = null;
		}
		else
			Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
		EmitSignal(SignalName.CameraDrag, cameraDragEnabled);
	}

	private void SetTileTooltip(Button button, Buyable tile)
	{		
		button.TooltipText = "Price: " + tile.Price + "$";
	}

	private void SelectTileType(Tile type)
	{
		mode = Mode.Tile;
		ChangeDragState(false);
		selectedTile = type;
		EmitSignal(SignalName.SelectedTileChanged, type);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton mouseEvent 
			&& mouseEvent.ButtonIndex == MouseButton.Left
			&& mouseEvent.Pressed
			&& !cameraDragEnabled)
		{
			switch (mode)
			{
				case Mode.Tile:
					EmitSignal(SignalName.PlaceTile, selectedTile);
					break;
				case Mode.Animal:
					break;
				case Mode.Jeep:
					EmitSignal(SignalName.JeepBought);
					break;
				default:
					break;
			}
		}
	}
}
