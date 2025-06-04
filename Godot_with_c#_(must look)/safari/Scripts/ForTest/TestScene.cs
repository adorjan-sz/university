using Godot;
using Godot.NativeInterop;
using Safari.Scripts.Game.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


	public partial class TestScene : Node2D
{
	private NavigationRegion2D _navRegion;
	private MapManager _mapManager;
	private Vector2 TopLeft;
	private Vector2 BottomRight;
	private Vector2 BottomLeft;
	private Vector2 TopRight;
	private EntityManager _entityManager;
	public  override void _Ready()
	{
		_entityManager = GetNode<EntityManager>("EntityManager");
		//_navRegion = GetNode<NavigationRegion2D>("NavigationRegion2D");
		/*
	   
		var newNavigationMesh = new NavigationPolygon();

		TopLeft = _mapManager.ToGlobal(_mapManager.Layer1.MapToLocal(new Vector2I(-1, -1)));
		BottomRight = _mapManager.ToGlobal(_mapManager.Layer1.MapToLocal(new Vector2I(_mapManager.Width, _mapManager.Height)));
		BottomLeft = _mapManager.ToGlobal(_mapManager.Layer1.MapToLocal(new Vector2I(0, _mapManager.Height)));
		TopRight = _mapManager.ToGlobal(_mapManager.Layer1.MapToLocal(new Vector2I(_mapManager.Width, 0)));

		Vector2[] corners = { TopLeft, BottomLeft, BottomRight, TopRight };
		newNavigationMesh.AddOutline(corners);

		NavigationServer2D.BakeFromSourceGeometryData(newNavigationMesh, new NavigationMeshSourceGeometryData2D());
		_navRegion.NavigationPolygon = newNavigationMesh;
		*/

	}
   
}
