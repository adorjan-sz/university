using Godot;
using System;
using System.Collections.Generic;

[Serializable]
public class CarnivoreSave : AnimalSave
{
    // Food is now stored by Ids, not UsableTiles
    public List<long> FoodIds { get; set; } = new();

    // Current target animal to hunt (by Id)
    public long? CurrentTargetId { get; set; }
}
