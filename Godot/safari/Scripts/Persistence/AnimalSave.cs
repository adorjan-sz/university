using Godot;
using System;
using System.Collections.Generic;

[Serializable]
public class AnimalSave
{
    public string AnimalType { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public long Id { get; set; }

    public float? GroupX { get; set; }
    public float? GroupY { get; set; }

    public List<int> WaterTileXs { get; set; } = new();
    public List<int> WaterTileYs { get; set; } = new();

   

    public int? CurrentWaterX { get; set; }
    public int? CurrentWaterY { get; set; }

   

    public float? CurrentGroupUpX { get; set; }
    public float? CurrentGroupUpY { get; set; }

    public string CurrentStateName { get; set; }
    public double Hunger { get; set; }
    public double Thirst { get; set; }
    public double Age { get; set; }
    public int Count { get; set; }
    public bool IsHuntGoingOn { get; set; }
    

}
