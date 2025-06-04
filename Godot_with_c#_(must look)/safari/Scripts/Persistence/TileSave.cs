using Godot;
using System;
using System.Text.Json;
using Safari.Scripts.Game.Tiles;

[Serializable]
public class TileSave
{
    public int X { get; set; }
    public int Y { get; set; }
    public string TileType { get; set; }
    public int Layer { get; set; }

    public TileSave() { } 

    public TileSave(Vector2I position, Tile tile, int layer)
    {
        X = position.X;
        Y = position.Y;
        TileType = tile.GetType().Name;
        Layer = layer;
    }
    /*
    public Vector2I Position
    {
        get => new Vector2I(X, Y);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }*/
}
