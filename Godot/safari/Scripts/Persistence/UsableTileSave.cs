using Godot;
using System;
using Safari.Scripts.Game.Entities.Animals.UsableTiles;

[Serializable]
public class UsableTileSave
{
    public string TileTypeName { get; set; }

    public int MapX { get; set; }
    public int MapY { get; set; }

    public int WorldX { get; set; }
    public int WorldY { get; set; }
    public UsableTileSave() { } // Parameterless constructor for serialization
    public UsableTileSave(UsableTile usableTile)
    {
        TileTypeName = usableTile.TileType.GetType().Name;

        MapX = usableTile.MapCoords.X;
        MapY = usableTile.MapCoords.Y;

        WorldX = usableTile.WorldCoords.X;
        WorldY = usableTile.WorldCoords.Y;
    }

    
}
