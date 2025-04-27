using System.Collections.Generic;
using Godot;
using System;
using System.Text.Json;
[Serializable]
public class GameSaveData
{
	public List<TileSave> Tiles { get; set; } = new();
	public List<UsableTileSave> UsableTiles { get; set; } = new();
	public List<HerbivoreSave> Herbivores { get; set; } = new();
	public List<CorpseSave> Corpse { get; set; } = new();
	public List<CarnivoreSave> Carnivores { get; set; } = new();
	public List<JeepSave> Jeeps { get; set; } = new();
	public LightSave Lights { get; set; } = new();
	public int Money { get; set; }
	// public float Time { get; set; }
}
