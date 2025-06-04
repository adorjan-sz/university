using Godot;
using System;
using System.Collections.Generic;

namespace Safari.Scripts.Game.Tiles
{
	public abstract partial class RandomTextureTile : Tile
	{
		private List<Vector2I> _atlasCoordinateOptions;
		public RandomTextureTile(int layer, int atlasId, List<Vector2I> options, bool isPassable, bool lightSource) 
			: base(layer, atlasId, options[0], isPassable, lightSource)
		{
			if (options == null || options.Count == 0)
				throw new ArgumentException("Options list cannot be null or empty.", nameof(options));
			_atlasCoordinateOptions = [];
			_atlasCoordinateOptions.AddRange(options);
			UpdateAtlasCoordinateRandom();
		}

		protected void UpdateAtlasCoordinateRandom()
		{
			Random random = new Random();
			int index = random.Next(_atlasCoordinateOptions.Count);
			AtlasCoord = _atlasCoordinateOptions[index];
		}
	}
}
