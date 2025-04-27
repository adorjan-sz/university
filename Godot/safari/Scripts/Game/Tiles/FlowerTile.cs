using Godot;

namespace Safari.Scripts.Game.Tiles
{
    public partial class FlowerTile : Tile, Buyable
    {
        private int _price;
        int Buyable.Price { get => _price; }

        public FlowerTile(bool lightSource) : base(1, 1, new Vector2I(8, 3), false, lightSource)
        {
            _price = 10;
        }
    }
}
