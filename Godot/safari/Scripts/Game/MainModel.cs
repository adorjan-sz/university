using Godot;

using System;
using System.IO;
using System.Text.Json; // Kell a JSON serializationhez
using System.Collections.Generic;

public partial class MainModel : Node2D
{
	
	[Export] public EntityManager entityManager;
	public string ParkName = "Debug";
	public MapManager mapManager;
	private string SavePath => $"user://{ParkName}.json";
	public override void _Ready()
	{
		base._Ready();
		mapManager = entityManager.GetMapManager();
	   
	}
	public void NewGame()
	{
		// Initialize the game state here
		entityManager.NewGame();
		GD.Print("New game started!");
	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("save_game"))
		{
			GD.PrintErr("Saving game...");
			SaveGame();
			
		}
		if (Input.IsActionJustPressed("load_game_debug"))
		{
			GD.PrintErr("Loading game...");
			LoadGame();

		}
	}

	private void SaveGame()
	{
		
		GameSaveData saveData = new GameSaveData();

		
		saveData.Tiles = mapManager.SaveMap();
		GD.PrintErr(saveData.Tiles.Count);
		
		saveData.UsableTiles = entityManager.SaveUsableTiles();

		
		saveData.Herbivores = entityManager.SaveHerbivores();
		saveData.Corpse = entityManager.SaveCorpses();
		saveData.Carnivores = entityManager.SaveCarnivores();
		saveData.Jeeps = entityManager.SaveJeeps();
		saveData.Lights = entityManager.SaveLight();
		saveData.Money = GameVariables.Instance.GetMoney();
		/*
		
		saveData.Money = 0;//GameVariables.Instance.Money;
		saveData.Time = 0;//GameVariables.Instance.Time;
		*/
		string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
		GD.PrintErr("json tartalma"+json);
		DirAccess.MakeDirRecursiveAbsolute(SavePath.GetBaseDir());
		Godot.FileAccess file = Godot.FileAccess.Open(SavePath, Godot.FileAccess.ModeFlags.Write);
		
		if (file.StoreString(json))
		{

			GD.PrintErr("Game saved successfully to"+ file.GetPath());
		}
		file.Close();

		GD.Print("Game saved!");
	}
	public void LoadGame(string _parkName="Debug")
	{
		ParkName = _parkName;
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

		GD.PrintErr(saveData.Tiles[100].X);
		mapManager.LoadMap(saveData.Tiles);

	   
		entityManager.LoadUsableTiles(saveData.UsableTiles);
		

	   
		entityManager.LoadHerbivores(saveData.Herbivores);
		entityManager.LoadCorpses(saveData.Corpse);
		entityManager.LoadCarnivores(saveData.Carnivores);
		entityManager.LoadJeeps(saveData.Jeeps);
		entityManager.LoadLight(saveData.Lights);
		GameVariables.Instance.SetMoney(saveData.Money);

		GD.Print("Game loaded!");
	}

}
