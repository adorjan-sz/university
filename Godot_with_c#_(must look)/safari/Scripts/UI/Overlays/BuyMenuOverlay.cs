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
	[Export] public Button StagButton;
	[Export] public Button BoarButton;
	[Export] public Button WolfButton;
	[Export] public Button HyenaButton;
	[Export] public Button ChipButton;
	[Export] public Button RoadButton;
	[Export] public Button JeepButton;
	[Export] public Button SellButton;
	[Export] public Button DragButton;
	[Export] public SpinBox TicketPrice;

	[Signal] public delegate void PlaceTileEventHandler(Tile type);
	[Signal] public delegate void SelectedTileChangedEventHandler(Tile type);
	[Signal] public delegate void CameraDragEventHandler(bool enabled);
	[Signal] public delegate void JeepBoughtEventHandler();
	[Signal] public delegate void AnimalBoughtEventHandler(Animal animal);
	[Signal] public delegate void ChipBoughtEventHandler();

	private Tile selectedTile;
	private Animal selectedAnimal;
	private bool cameraDragEnabled;

	private enum Mode {Tile, Animal, Jeep, Chip };
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
		StagButton.Pressed += () => SelectAnimalType(new Stag());
		BoarButton.Pressed += () => SelectAnimalType(new Boar());
		WolfButton.Pressed += () => SelectAnimalType(new Wolf());
		HyenaButton.Pressed += () => SelectAnimalType(new Hyena());
		ChipButton.Pressed += OnChipButtonPressed;

		TicketPrice.ValueChanged += OnTicketPriceChanged;
		TicketPrice.Value = GameVariables.Instance.TicketPrice;

		SetButtonTooltip(FlowerButton, new FlowerTile(true));
		SetButtonTooltip(LogButton, new LogTile(true));
		SetButtonTooltip(RockButton, new RockTile(true));
		SetButtonTooltip(WaterButton, new WaterTile());
		SetButtonTooltip(TreeButton, new TreeTile(TreeVariant.Full, true));
		SetButtonTooltip(RoadButton, new RoadTile());
		SetButtonTooltip(StagButton, new Stag());
		SetButtonTooltip(BoarButton, new Boar());
		SetButtonTooltip(WolfButton, new Wolf());
		SetButtonTooltip(HyenaButton, new Hyena());
		SetButtonTooltip(ChipButton, new TrackerChip());
		SetButtonTooltip(JeepButton, new Jeep());
		ChangeDragState(true);
	}

	private void SelectAnimalType(Animal animal)
	{
		mode = Mode.Animal;
		ChangeDragState(false);
		selectedAnimal = animal;
		EmitSignal(SignalName.SelectedTileChanged, new EmptyTile());
	}

	private void OnTicketPriceChanged(double value)
	{
		GameVariables.Instance.SetTicketPrice((int)value);
	}

	private void OnChipButtonPressed(){
		mode = Mode.Chip;
		ChangeDragState(false);
		EmitSignal(SignalName.SelectedTileChanged, new EmptyTile());
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

	private void SetButtonTooltip(Button button, Buyable tile)
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
		if (GameVariables.Instance.IsGameOver)
			return;
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
					EmitSignal(SignalName.AnimalBought, selectedAnimal);
					break;
				case Mode.Jeep:
					EmitSignal(SignalName.JeepBought);
					break;
				case Mode.Chip:
					EmitSignal(SignalName.ChipBought);
					break;
				default:
					break;
			}
		}
	}
}
