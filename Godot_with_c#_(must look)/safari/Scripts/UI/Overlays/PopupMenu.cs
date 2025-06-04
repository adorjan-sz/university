using Godot;
using System;
using System.Linq;
using System.Runtime.InteropServices;

public partial class PopupMenu : Control
{
	[Signal] public delegate void SaveButtonPressedEventHandler();
	

	[Export] public Button SaveButton;
	[Export] public Button MainScreenButton;
	[Export] public Button ExitButton;
	[Export] public NodePath TopBarPath;
	[Export] public NodePath MainModelPath;
	[Export] public StyleBoxFlat ButtonStyle;

	private Tween _tween;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TopBar topBar = GetNode<TopBar>(TopBarPath);
		MainModel mainModel = GetNode<MainModel>(MainModelPath);

		//Connect signals
		topBar.PopupMenu += OnPopupMenu;
		mainModel.SaveCompleted += OnSaveCompleted;
		SaveButton.Pressed += OnSaveButton;
		
		MainScreenButton.Pressed += OnMainScreenButton;
		ExitButton.Pressed += OnExitButton;
	}

	/// <summary>
	/// Toggles this popup's visibility.
	/// </summary>
	private void OnPopupMenu(bool visibility)
	{
		this.Visible = visibility;
	}
	/// <summary>
	/// Handles a button color feedback animation when saving is completed.
	/// The button color smoothly transitions to green (success) or red (failure),
	/// then returns back to the original style.
	/// </summary>
	/// <param name="successful"></param>
	private void OnSaveCompleted(bool successful)
	{
		if (_tween != null && _tween.IsRunning())
			_tween.Kill();

		StyleBoxFlat style = SaveButton.GetThemeStylebox("hover") as StyleBoxFlat;

		SaveButton.AddThemeStyleboxOverride("hover", style);

		Color targetColor = successful ? new Color(0, 0.41f, 0) : new Color(0.41f, 0, 0);
		Color originalColor = style.BgColor;

		_tween = CreateTween();
		_tween.TweenProperty(style, "bg_color", targetColor, 0.2f);
		_tween.TweenProperty(style, "bg_color", originalColor, 0.3f);
		_tween.TweenCallback(Callable.From(() =>
		{
			SaveButton.RemoveThemeStyleboxOverride("hover");
		}));
	}

	/// <summary>
	/// Emits a save event.
	/// </summary>
	private void OnSaveButton()
	{
		EmitSignal(SignalName.SaveButtonPressed);
	}

	/// <summary>
	/// Emits a load event.
	/// </summary>
	

	/// <summary>
	/// Returns to the main menu by unloading this popup and changing scene.
	/// </summary>
	private void OnMainScreenButton()
	{
		Visible = false;
		GetTree().Root.ProcessMode = ProcessModeEnum.Always;
		SceneTree tree = GetTree();
		tree.Root.PrintTreePretty();
		Control mainMenu = tree.Root.FindChild("MainMenu", true, false) as Control;
		mainMenu.Visible = true;
		tree.CurrentScene = mainMenu;
		tree.Root.FindChild("MainModel", true, false).QueueFree();
	}

	/// <summary>
	/// Quits the application.
	/// </summary>
	private void OnExitButton()
	{
		GetTree().Quit();
	}

}
