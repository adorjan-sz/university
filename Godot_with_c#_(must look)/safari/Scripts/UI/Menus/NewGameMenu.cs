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
		_parkNameInput = GetNode<LineEdit>("Panel/MarginContainer/FormContainer/ParkNameInput");
		_difficultyDropdown = GetNode<OptionButton>("Panel/MarginContainer/FormContainer/DifficultyDropdown");
		_startGameButton = GetNode<Button>("Panel/MarginContainer/FormContainer/StartGameButton");
		_backToMenuButton = GetNode<Button>("Panel/MarginContainer/FormContainer/BackToMenuButton");

		// Nehézségi szintek hozzáadása
		_difficultyDropdown.AddItem("Easy", 0);
		_difficultyDropdown.AddItem("Normal", 1);
		_difficultyDropdown.AddItem("Hard", 2);

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
		GameVariables.Instance.ParkName = parkName;

		GD.Print($"Új játék indítása: {parkName}, nehézség: {difficulty}");

		// Játék indítása a megfelelő nehézséggel és parknévvel
		if (MainModelScene != null)
		{
			// Instantiate the new scene
			var _mainModel = (MainModel)MainModelScene.Instantiate();
			switch(difficulty)
			{
				case 0:
					_mainModel.SetDifficulty(Difficulty.Easy);
					break;
				case 1:
					_mainModel.SetDifficulty(Difficulty.Medium);
					break;
				case 2:
					_mainModel.SetDifficulty(Difficulty.Hard);
					break;
			}
			// Get the root of the scene tree
			SceneTree tree = GetTree();

			// Set the new scene as the current scene
			tree.Root.AddChild(_mainModel);
			tree.CurrentScene = _mainModel;
			_mainModel.NewGame();
			// Remove the current scene
			tree.Root.FindChild("NewGame", true, false).QueueFree();
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
		SceneTree tree = GetTree();
		tree.Root.PrintTreePretty();
		Control mainMenu = tree.Root.FindChild("MainMenu", true, false) as Control;
		mainMenu.Visible = true;
		tree.CurrentScene = mainMenu;
		tree.Root.FindChild("NewGame", true, false).QueueFree();
	}
}
