using Godot;

namespace Safari.Scripts.Game.Tiles
{
	public partial class RockTile : Tile, Buyable
	{
		private int _price;
		int Buyable.Price { get => _price; }

		public RockTile(bool lightSource) : base(1, 1, new Vector2I(9, 4), false, lightSource)
		{

			_price = 20;
		}
	}
}
