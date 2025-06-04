using System;
using System.Collections.Generic;

[Serializable]
public class JeepSave
{
    public List<TouristSave> Passengers { get; set; } = new();
    public int PosX { get; set; }
    public int PosY { get; set; }
    public string State { get; set; } // Store the enum as int
    public int NextCellX { get; set; }
    public int NextCellY { get; set; }
    public JeepSave() { } // Parameterless constructor for serialization
    public JeepSave(Jeep jeep)
    {
        Passengers = jeep.SaveTourist();
        PosX = jeep.Position.X;
        PosY = jeep.Position.Y;
        State = jeep.StateString;
        NextCellX = jeep.NextCell.X;
        NextCellY = jeep.NextCell.Y;
    }
}
