using Godot;
using System;

public partial class TimeControlOverlay : GridContainer
{
	[Export] public Texture2D PlayIcon;
	[Export] public Texture2D PauseIcon;
	[Export] public Button TogglePauseButton;
	[Export] public NodePath TopBarPath;

	[Export] public float MinTimeScale = 0.25f;
	[Export] public float MaxTimeScale = 64f;
	[Export] public float TimeScaleStep = 2f;

	
	private bool isPaused = false;
	private float lastTimeScale = 1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TopBar topBar = GetNode<TopBar>(TopBarPath);
		topBar.PopupMenu += OnPopupMenu;

		lastTimeScale = (float)Engine.TimeScale;
		UpdatePauseButtonIcon();
	}

	// Updates the button icon based on the pause state
	private void UpdatePauseButtonIcon()
	{
		if (GameVariables.Instance.IsGameOver)
			return;
		if (TogglePauseButton != null)
			TogglePauseButton.Icon = isPaused ? PlayIcon : PauseIcon;
	}

	// Slows down the game speed
	public void _on_slow_down_btn_pressed()
	{
		if (GameVariables.Instance.IsGameOver)
			return;
		Engine.TimeScale = Mathf.Max(MinTimeScale, (float)Engine.TimeScale / TimeScaleStep);
		lastTimeScale = (float)Engine.TimeScale;
	}

	// Toggles pause and updates processing mode
	public void _on_pause_time_btn_pressed()
	{
		if (GameVariables.Instance.IsGameOver)
			return;
		isPaused = !isPaused;
		UpdatePauseButtonIcon();
		GetTree().Root.ProcessMode = isPaused ? ProcessModeEnum.Disabled : ProcessModeEnum.Always;
	}

	// Speeds up the game speed
	public void _on_speed_up_btn_pressed()
	{
		if (GameVariables.Instance.IsGameOver)
			return;
		Engine.TimeScale = Mathf.Min(MaxTimeScale, (float)Engine.TimeScale * TimeScaleStep);
		lastTimeScale = (float)Engine.TimeScale;
	}

	//Toggles pause and updates processing mode
	private void OnPopupMenu(bool state)
	{
		if (GameVariables.Instance.IsGameOver)
			return;
		isPaused = state;
		UpdatePauseButtonIcon();
		GetTree().Root.ProcessMode = isPaused ? ProcessModeEnum.Disabled : ProcessModeEnum.Always;
	}
}
