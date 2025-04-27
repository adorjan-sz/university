using Godot;

namespace Safari.Scripts.Game.Tiles
{
    public partial class LogTile : Tile, Buyable
    {
        private int _price;
        int Buyable.Price { get => _price; }

        public LogTile(bool lightSource) : base(1, 1, new Vector2I(6, 4), false, lightSource)
        {
            _price = 20;
        }
    }
}
