using Godot;
using System;

public partial class TimeControlOverlay : GridContainer
{
	[Export] public Texture2D PlayIcon;
	[Export] public Texture2D PauseIcon;
	[Export] public Button TogglePauseButton;

	[Export] public float MinTimeScale = 0.25f;
	[Export] public float MaxTimeScale = 4f;
	[Export] public float TimeScaleStep = 2f;

	private bool isPaused = false;
	private float lastTimeScale = 1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lastTimeScale = (float)Engine.TimeScale;
		UpdatePauseButtonIcon();
	}

	// Updates the button icon based on the pause state
	private void UpdatePauseButtonIcon()
	{
		if (TogglePauseButton != null)
			TogglePauseButton.Icon = isPaused ? PlayIcon : PauseIcon;
	}

	// Slows down the game speed
	public void _on_slow_down_btn_pressed()
	{
		Engine.TimeScale = Mathf.Max(MinTimeScale, (float)Engine.TimeScale / TimeScaleStep);
		lastTimeScale = (float)Engine.TimeScale;
	}

	// Toggles pause and updates processing mode
	public void _on_pause_time_btn_pressed()
	{
		isPaused = !isPaused;
		UpdatePauseButtonIcon();
		GetTree().Root.GetChild(0).ProcessMode = isPaused ? ProcessModeEnum.Disabled : ProcessModeEnum.Inherit;
	}

	// Speeds up the game speed
	public void _on_speed_up_btn_pressed()
	{
		Engine.TimeScale = Mathf.Min(MaxTimeScale, (float)Engine.TimeScale * TimeScaleStep);
		lastTimeScale = (float)Engine.TimeScale;
	}
}
