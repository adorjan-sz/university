using Godot;
using System;

public partial class NewGameMenu : Control
{
	private LineEdit _parkNameInput;
	private OptionButton _difficultyDropdown;
	private Button _startGameButton;
	private Button _backToMenuButton;
	[Export] public PackedScene MainModelScene;

	public override void _Ready()
	{
		
		// Csomópontok beállítása
		_parkNameInput = GetNode<LineEdit>("FormContainer/ParkNameInput");
		_difficultyDropdown = GetNode<OptionButton>("FormContainer/DifficultyDropdown");
		_startGameButton = GetNode<Button>("FormContainer/StartGameButton");
		_backToMenuButton = GetNode<Button>("FormContainer/BackToMenuButton");

		// Nehézségi szintek hozzáadása
		_difficultyDropdown.AddItem("Könnyű", 0);
		_difficultyDropdown.AddItem("Normál", 1);
		_difficultyDropdown.AddItem("Nehéz", 2);

		// Eseménykezelők beállítása
		_startGameButton.Pressed += OnStartGamePressed;
		_backToMenuButton.Pressed += OnBackToMenuPressed;
	}

	private void OnStartGamePressed()
	{
		string parkName = _parkNameInput.Text;
		int difficulty = _difficultyDropdown.GetSelectedId();

		if (string.IsNullOrWhiteSpace(parkName))
		{
			GD.Print("A park neve nem lehet üres!");
			return;
		}

		GD.Print($"Új játék indítása: {parkName}, nehézség: {difficulty}");

		// Játék indítása a megfelelő nehézséggel és parknévvel
		if (MainModelScene != null)
			{
				// Instantiate the new scene
				var _mainModel = (MainModel)MainModelScene.Instantiate();
				_mainModel.ParkName = parkName;
				
				// Get the root of the scene tree
				SceneTree tree = GetTree();

				// Remove the current scene
				tree.CurrentScene.QueueFree();

				// Set the new scene as the current scene
				tree.Root.AddChild(_mainModel);
				tree.CurrentScene = _mainModel;
			_mainModel.NewGame();
		}
			else
			{
				GD.PrintErr("MainModelScene is not assigned in the Inspector!");
			}

		//GetTree().ChangeSceneToFile("res://Scenes/MainModel.tscn");
	}

	private void OnBackToMenuPressed()
	{
		GD.Print("Visszalépés a főmenübe...");
		GetTree().ChangeSceneToFile("res://Scenes/UI/Menus/MainMenu.tscn");
	}
}
