using Godot;

namespace Safari.Scripts.Game.Tiles
{
	public partial class TreeTile : Tile, Buyable
	{
		public float GrowthProgress { get; private set; }
		public float GrowthThreshold { get; private set; }

		private TreeVariant _variant;

		public TreeVariant Variant
		{
			get { return _variant; }
			set
			{
				_variant = value;
				UpdateAtlasCoordinate();
			}
		}

		private int _price;
		int Buyable.Price { get { return _price; } }

		public TreeTile(TreeVariant variant, bool lightSource) : base(1, 2, GetAtlasCoordinateForVariant(variant), false, lightSource)
		{
			_price = 20;
			GrowthProgress = 0.0f;
			GrowthThreshold = 10.0f;
			_variant = variant;
		}

		private void UpdateAtlasCoordinate()
		{
			AtlasCoord = GetAtlasCoordinateForVariant(_variant);
		}

		private static Vector2I GetAtlasCoordinateForVariant(TreeVariant variant)
		{
			switch (variant)
			{
				case TreeVariant.Full:
					return new Vector2I(0, 0); //Full
				case TreeVariant.Partial:
					return new Vector2I(1, 0); //Partial
				case TreeVariant.Minimal:
					return new Vector2I(2, 0); //Minimal
				case TreeVariant.Bare:
					return new Vector2I(3, 0); //Bare 
				default:
					return new Vector2I(0, 0); //Full
			}
		}

		public void Grow(float delta)
		{
			GrowthProgress += delta;
			if (GrowthProgress >= GrowthThreshold && Variant != TreeVariant.Full)
			{
				Variant = (TreeVariant)((int)Variant + 1);
				GrowthProgress = 0.0f;
				UpdateAtlasCoordinate();
			}
		}

		public void Eat()
		{
			if (IsEdible())
			{
				if (Variant != TreeVariant.Bare)
				{
					Variant = (TreeVariant)((int)Variant - 1);
					GrowthProgress = 0.0f;
					UpdateAtlasCoordinate();
				}
			}
		}

		public bool IsEdible()
		{
			return Variant != TreeVariant.Bare;
		}
	}
}
