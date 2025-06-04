using Godot;

namespace Safari.Scripts.Game.Tiles
{
	public partial class WaterTile : Tile, Buyable
	{
		private int _price;
		int Buyable.Price { get => _price; }

		public WaterTile() : base(0, 1, new Vector2I(0, 10), false, false)
		{
			_price = 50;
		}
	}
}
