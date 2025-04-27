using Godot;
using System;
using System.Collections.Generic;

[Serializable]
public class HerbivoreSave : AnimalSave
{
  

    public List<int> FoodTileXs { get; set; } = new();
    public List<int> FoodTileYs { get; set; } = new();

   

    public int? CurrentVegetationX { get; set; }
    public int? CurrentVegetationY { get; set; }

   
    }
