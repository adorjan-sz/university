using Godot;
using Safari.Scripts.Game.Tiles;
using System;
using System.Data;

public partial class MiniMap : Control
{
	[Signal] public delegate void MinimapClickedEventHandler(Vector2 position);

	[Export] public TileMapLayer Layer0;
	[Export] public TileMapLayer Layer1;
	[Export] public Panel viewRect;
	[Export] public NodePath mapPath;
	[Export] public NodePath cameraPath;

	private MapManager mapManager;
	private CameraController camera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mapManager = GetNode<MapManager>(mapPath);
		mapManager.MapChanged += OnMapChanged;

		camera = GetNode<CameraController>(cameraPath);
		camera.CameraChanged += OnCameraChanged;

		int diag = mapManager.Width + mapManager.Height;
		int mapWidthInPixel = diag * 16;
		int mapHeightInPixel = diag * 8;
		Layer0.Scale = Size / mapWidthInPixel;
		Layer1.Scale = Layer0.Scale;
		Layer0.Position = new Vector2(Size.X * mapManager.Height / diag, 0);
		Layer1.Position = Layer0.Position;
		Size = new Vector2(Size.X, mapHeightInPixel * Layer0.Scale.Y);
		Position = GetParentAreaSize() - Size;


		for (int i = 0; i < mapManager.Width; ++i)
		{
			for (int j = 0; j < mapManager.Height; ++j)
			{
				Vector2I pos = new Vector2I(i, j);
				Layer0.SetCell(pos, mapManager.Layer0.GetCellSourceId(pos), mapManager.Layer0.GetCellAtlasCoords(pos));
				Layer1.SetCell(pos, mapManager.Layer1.GetCellSourceId(pos), mapManager.Layer1.GetCellAtlasCoords(pos));
			}
		}
	}

	private void OnMapChanged(Vector2I pos, int layer, Tile tile)
	{
		if (layer == 0)
			Layer0.SetCell(pos, tile.AtlasId, tile.AtlasCoord);
		else if (layer == 1)
			Layer1.SetCell(pos, tile.AtlasId, tile.AtlasCoord);
	}

	private void OnCameraChanged(Vector2 position, Vector2 zoom)
	{
		viewRect.Size = DisplayServer.WindowGetSize() / zoom * Layer0.Scale;
		Vector2 cameraCenter = position - mapManager.Position;
		Vector2I minimapCameraCenter = mapManager.Layer0.LocalToMap(cameraCenter);
		Vector2 rectCenter = Layer0.MapToLocal(minimapCameraCenter) * Layer0.Scale + Layer0.Position;
		Vector2 rectPos = rectCenter - viewRect.Size / 2;
		Vector2 rectPosC = new Vector2(Mathf.Clamp(0, rectPos.X, Size.X), Mathf.Clamp(0, rectPos.Y, Size.Y));
		viewRect.Size = rectPos + viewRect.Size - rectPosC;
		viewRect.Position = rectPosC;
	}

	// Handle user input on the minimap.
	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				Vector2 localPos = Layer0.GetLocalMousePosition();
				Vector2I localClick = Layer0.LocalToMap(localPos);
				Vector2 cameraPos = mapManager.Layer0.MapToLocal(localClick) + mapManager.Position;
				EmitSignal(SignalName.MinimapClicked, cameraPos);
			}
		}
	}
}
