using Safari.Scripts.Game.Tiles;
using System;

public static class TileFactory
{
    public static Tile CreateTile(string tileType)
    {
        return tileType switch
        {
            "DirtTile" => new DirtTile(),
            "WaterTile" => new WaterTile(),
            "GrassTile" => new GrassTile(),
            "RockTile" => new RockTile(false),
            "FlowerTile" => new FlowerTile(false),
            "LogTile" => new LogTile(false),
            "RoadTile" => new RoadTile(),
            "EmptyTile" => new EmptyTile(),
            _ => throw new Exception($"Unknown tile type: {tileType}")
        };
    }
}
