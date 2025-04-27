using Godot;
using Safari.Scripts.Game.Tiles;

namespace Safari.Scripts.Game
{
    public class MapData
    {
        public int Width { get; }
        public int Height { get; }
        public int Layers { get; }

        private Tile[,,] cells;

        public MapData(int width, int height, int layers)
        {
            Width = width;
            Height = height;
            Layers = layers;
            cells = new Tile[width, height, layers];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int layer = 0; layer < Layers; layer++)
                    {
                        cells[x, y, layer] = new EmptyTile();
                    }
                }
            }
        }

        public Tile GetTile(Vector2I pos, int layer)
        {
            if (!IsInBounds(pos, layer))
                return null;

            return cells[pos.X, pos.Y, layer];
        }

        public void SetTile(Vector2I pos, int layer, Tile tile)
        {
            if (!IsInBounds(pos, layer))
                return;

            cells[pos.X, pos.Y, layer] = tile;
        }

        public bool IsInBounds(Vector2I pos, int layer)
        {
            return pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height && layer >= 0 && layer < Layers;
        }

        public bool CanPlace(Vector2I pos, Tile tile)
        {
            if (!IsInBounds(pos, tile.Layer))
                return false;

            if (tile.Layer == 0)
            {
                if (cells[pos.X, pos.Y, 1] is EmptyTile)
                    return cells[pos.X, pos.Y, 0] is DirtTile || cells[pos.X, pos.Y, 0] is GrassTile;
                else
                    return false;
            }
            else if (tile.Layer == 1)
            {
                if (cells[pos.X, pos.Y, 0] is WaterTile || cells[pos.X, pos.Y, 1] is not EmptyTile)
                    return false;
                if (tile is FlowerTile || tile is LogTile || tile is TreeTile)
                    return cells[pos.X, pos.Y, 0] is GrassTile;
            }
            return true;
        }
    }
}
