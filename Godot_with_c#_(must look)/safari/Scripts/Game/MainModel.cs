using Godot;

using System;
using System.IO;
using System.Text.Json; 
using System.Collections.Generic;

public partial class MainModel : Node2D
{
	[Signal] public delegate void SaveCompletedEventHandler(bool successful);

	[Export] public EntityManager entityManager;
	private PopupMenu popupMenu;
	

	public MapManager mapManager;
	public Camera2D camera;
	private string SavePath => $"user://{GameVariables.Instance.ParkName}.json";
	public override void _Ready()
	{
		base._Ready();
		
		mapManager = entityManager.GetMapManager();
		popupMenu = GetNode<PopupMenu>("/root/MainModel/GameOverlay/Control/PopupMenu");
		camera = GetNode<Camera2D>("Camera2D");

		popupMenu.SaveButtonPressed += SaveGame;	

	}
	public void NewGame()
	{
		// Initialize the game state here
		entityManager.NewGame();
		GetTree().Root.PrintTreePretty();
		GD.Print("New game started!");
		GameVariables.Instance.NewGame();
	}
	public override void _Process(double delta)
	{
		
	}
	public void SetDifficulty(Difficulty difficulty)
	{
		entityManager.Difficulty = difficulty;
	}
	public void SaveGame()
	{
		if (GameVariables.Instance.IsGameOver)
			return;
		GameSaveData saveData = new GameSaveData();

		saveData.ParkName = GameVariables.Instance.ParkName;
		saveData.Tiles = mapManager.SaveMap();
		//GD.PrintErr(saveData.Tiles.Count);
		
		saveData.UsableTiles = entityManager.SaveUsableTiles();

		
		saveData.Herbivores = entityManager.SaveHerbivores();
		saveData.Corpse = entityManager.SaveCorpses();
		saveData.Carnivores = entityManager.SaveCarnivores();
		saveData.Jeeps = entityManager.SaveJeeps();
		saveData.Lights = entityManager.SaveLight();
		saveData.Money = GameVariables.Instance.GetMoney();
		saveData.ReviewAverage = entityManager.ReviewAverage;
		saveData.ReviewCount = entityManager.ReviewCount;
		saveData.AnimalId = entityManager.GetSetAnimalId;
		saveData.TouristCount = entityManager.SaveTourist();
		saveData.TicketPrice = GameVariables.Instance.StartingTicketPrice;
		saveData.GameDifficulty = entityManager.Difficulty.ToString();
		saveData.Day = entityManager.DayCounter;


		bool success = false;

		string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = false });
		DirAccess.MakeDirRecursiveAbsolute(SavePath.GetBaseDir());
		Godot.FileAccess file = Godot.FileAccess.Open(SavePath, Godot.FileAccess.ModeFlags.Write);
		if (file != null)
		{
			if (file.StoreString(json))
			{
				GD.PrintErr("Game saved successfully to " + file.GetPath());
				success = true;
			}
			else
				GD.PrintErr("Failed to save the game!");
			file.Close();
		}
		else
			GD.PrintErr("Failed to open file for saving!");

		EmitSignal(SignalName.SaveCompleted, success);
	}
	public void LoadGame(string _parkName="Debug")
	{
		
		if (!Godot.FileAccess.FileExists(SavePath))
		{
			GD.PrintErr("No save file found!");
			return;
		}

		Godot.FileAccess file = Godot.FileAccess.Open(SavePath, Godot.FileAccess.ModeFlags.Read);
		string json = file.GetAsText();
		file.Close();

		GameSaveData saveData = JsonSerializer.Deserialize<GameSaveData>(json);

		if (saveData == null)
		{

			GD.PrintErr("Failed to deserialize save data!");
			return;
		}

		GameVariables.Instance.ParkName = saveData.ParkName;
		mapManager.LoadMap(saveData.Tiles);

	   
		entityManager.LoadUsableTiles(saveData.UsableTiles);
		

	   
		entityManager.LoadHerbivores(saveData.Herbivores);
		entityManager.LoadCorpses(saveData.Corpse);
		entityManager.LoadCarnivores(saveData.Carnivores);
		entityManager.LoadJeeps(saveData.Jeeps);
		entityManager.LoadLight(saveData.Lights);
		
		entityManager.ReviewAverage = saveData.ReviewAverage;
		entityManager.ReviewCount = saveData.ReviewCount;
		entityManager.GetSetAnimalId = saveData.AnimalId;
		GameVariables.Instance.SetMoney(saveData.Money);
		entityManager.LoadTourist(saveData.TouristCount);
		GameVariables.Instance.StartingTicketPrice = saveData.TicketPrice;
		entityManager.Difficulty = (Difficulty)Enum.Parse(typeof(Difficulty), saveData.GameDifficulty);
		entityManager.DayCounter = saveData.Day;
		GameVariables.Instance.ContinueGame();
		GD.Print("Game loaded!");
	}

}
