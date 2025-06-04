using Godot;
using System;
using System.Linq;

public partial class TopBar : Control
{
	[Signal] public delegate void PopupMenuEventHandler(bool visibility);

	[Export] public Label MoneyLabel;
	[Export] public Label HerbivoreLabel;
	[Export] public Label CarnivoreLabel;
	[Export] public Label TouristLabel;
	[Export] public TextureProgressBar ReviewProgressBar;
	[Export] public Label TimeLabel;
	[Export] public Label ParkNameLabel;
	[Export] public Button MenuButton;
	[Export] public NodePath EntityMangerPath;
	[Export] public NodePath LightMangerPath;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EntityManager entityManager = GetNode<EntityManager>(EntityMangerPath);
		entityManager.TouristCountChanged += OnTouristCountChanged;
		entityManager.TouristReviewChanged += OnTouristReviewChanged;
		entityManager.AnimalCountChanged += OnAnimalCountChanged;

		LightManager lightManager = GetNode<LightManager>(LightMangerPath);
		lightManager.TimeChanged += OnTimeChanged;

		GameVariables.Instance.MoneyChanged += OnMoneyChanged;

		MenuButton.Pressed += OnMenuButtonPressed;
		
		//Set default values
		MoneyLabel.Text = GameVariables.Instance.GetMoney().ToString();
		TimeLabel.Text = FormatTime(LightManager.DayStart);
		ParkNameLabel.Text = GameVariables.Instance.ParkName;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// Updates the money display label when the player's money changes.
	/// </summary>
	/// <param name="newAmount">The new amount of money to display.</param>
	private void OnMoneyChanged(int newAmount)
	{
		MoneyLabel.Text = newAmount.ToString();
	}

	/// <summary>
	/// Updates the tourist count label when the number of waiting tourists changes.
	/// </summary>
	/// <param name="count">The current number of tourists waiting.</param>
	private void OnTouristCountChanged(int count)
	{
		TouristLabel.Text = count.ToString();
	}

	private void OnTouristReviewChanged(double average)
	{
		ReviewProgressBar.Value = average;
	}

	/// <summary>
	/// Updates the animal count label when the total number of animals changes.
	/// </summary>
	/// <param name="herbivores">The current total number of herbivore animals in the park.</param>
	/// <param name="carnivores">The current total number of carnivore animals in the park.</param>
	private void OnAnimalCountChanged(int herbivores, int carnivores)
	{
		HerbivoreLabel.Text = herbivores.ToString();
		CarnivoreLabel.Text = carnivores.ToString();
	}

	/// <summary>
	/// Updates the time display label when the game time advances.
	/// </summary>
	/// <param name="time">The current game time in seconds.</param>
	private void OnTimeChanged(double time)
	{
		TimeLabel.Text = FormatTime(time);
	}

	/// <summary>
	/// Formats a time in seconds into MM:SS with zero padding.
	/// </summary>
	/// <param name="time">Time in seconds to format.</param>
	/// <returns>A string in the format "MM:SS".</returns>
	private string FormatTime(double time)
	{
		int t = Mathf.FloorToInt(time);
		return $"{t / 60:D2}:{t % 60:D2}";
	}

	private void OnMenuButtonPressed()
	{
		EmitSignal(SignalName.PopupMenu, MenuButton.ButtonPressed);
	}
	public override void _ExitTree()
	{
		if (GameVariables.Instance != null)
		{
			GameVariables.Instance.MoneyChanged -= OnMoneyChanged;

		}
	}

}
