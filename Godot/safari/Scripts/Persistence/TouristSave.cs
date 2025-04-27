using System;
using System.Collections.Generic;
using Safari.Scripts.Game.Entities;

[Serializable]
public class TouristSave
{
    public int TotalAnimalSeen { get; set; }
    public Dictionary<string, int> SpeciesCounts { get; set; } = new();
    public TouristSave(Tourist tourist)
    {
        TotalAnimalSeen = tourist.TotalAnimalSeen;
        SpeciesCounts = new Dictionary<string, int>(tourist.SpeciesCounts);
    }
    public TouristSave()
    {
        
    }
}
