using Godot;
using System;

public partial class MainMenu : Control
{
	private Button _newGameButton;
	private Button _loadGameButton;
	private Button _exitButton;
	[Export] public PackedScene LoadGameMenu;
	[Export] public PackedScene NewGameMenuSc;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		 // Gombok referenciáinak beállítása
		_newGameButton = GetNode<Button>("Panel/ButtonContainer/NewGameButton");
		_loadGameButton = GetNode<Button>("Panel/ButtonContainer/LoadGameButton");
		_exitButton = GetNode<Button>("Panel/ButtonContainer/Exit");

		// Gombok eseménykezelőinek beállítása
		_newGameButton.Pressed += OnNewGamePressed;
		_loadGameButton.Pressed += OnLoadGamePressed;
		_exitButton.Pressed += OnExitPressed;
		GetTree().Root.PrintTreePretty();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void OnNewGamePressed()
	{
		GD.Print("Új játék indítása...");
		// Itt indíthatod az új játékot (példa: JátékScene betöltése)
		Visible = false;
		var newGame = (NewGameMenu)NewGameMenuSc.Instantiate();

		// Get the root of the scene tree
		SceneTree tree = GetTree();

		// Set the new scene as the current scene
		tree.Root.AddChild(newGame);
		tree.CurrentScene = newGame;
	}

	

	private void OnLoadGamePressed()
	{
		GD.Print("Játék betöltése...");
		Visible = false;
		var LoadMenu = (LoadGameMenu)LoadGameMenu.Instantiate();

		// Get the root of the scene tree
		SceneTree tree = GetTree();

		// Set the new scene as the current scene
		tree.Root.AddChild(LoadMenu);
		tree.CurrentScene = LoadMenu;
		
		//GetTree().ChangeSceneToFile("res://Scenes/UI/Menus/LoadGameMenu.tscn");
		
	}

	private void OnExitPressed()
	{
		GD.Print("Kilépés...");
		GetTree().Quit(); // Kilépés a játékból
	}
}
