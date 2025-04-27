using Godot;
using System;

public partial class MainMenu : Control
{
	private Button _newGameButton;
	private Button _continueButton;
	private Button _loadGameButton;
	private Button _exitButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		 // Gombok referenciáinak beállítása
		_newGameButton = GetNode<Button>("ButtonContainer/NewGameButton");
		_loadGameButton = GetNode<Button>("ButtonContainer/LoadGameButton");
		_exitButton = GetNode<Button>("ButtonContainer/Exit");

		// Gombok eseménykezelőinek beállítása
		_newGameButton.Pressed += OnNewGamePressed;
		_loadGameButton.Pressed += OnLoadGamePressed;
		_exitButton.Pressed += OnExitPressed;


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnNewGamePressed()
	{
		GD.Print("Új játék indítása...");
		// Itt indíthatod az új játékot (példa: JátékScene betöltése)
		GetTree().ChangeSceneToFile("res://Scenes/UI/Menus/NewgameMenu.tscn");
	}

	

	private void OnLoadGamePressed()
	{
		GD.Print("Játék betöltése...");
		GetTree().ChangeSceneToFile("res://Scenes/UI/Menus/LoadGameMenu.tscn");
		// Itt nyithatsz egy betöltési menüt
	}

	private void OnExitPressed()
	{
		GD.Print("Kilépés...");
		GetTree().Quit(); // Kilépés a játékból
	}
	

	
}
