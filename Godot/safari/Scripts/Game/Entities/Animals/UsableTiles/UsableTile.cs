using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Safari.Scripts.Game.Tiles;
using Safari.Scripts.Game;
namespace Safari.Scripts.Game.Entities.Animals.UsableTiles;


public partial class UsableTile 
{
	
	public Tile TileType;
	private Vector2I _worldCoords;
	private Vector2I _mapCoords;
	
	private double counter;
	
	
	public Vector2I WorldCoords
	{
		get { return _worldCoords; }
		set { _worldCoords = value; }
	}
	public Vector2I MapCoords
	{
		get { return _mapCoords; }
		set { _mapCoords = value; }
	}


	public UsableTile(Tile tile,Vector2I MapCoords,Vector2I worldCoords)
	{
		TileType = tile;
		_mapCoords = MapCoords;
		_worldCoords = worldCoords;
		

		
	}
	
}
