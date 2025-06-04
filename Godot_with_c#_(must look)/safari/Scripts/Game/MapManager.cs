using Godot;
using Safari.Scripts.Game;
using Safari.Scripts.Game.Road;
using Safari.Scripts.Game.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MapManager : Node2D
{
	private FastNoiseLite terrainGenerator = new FastNoiseLite();
	private FastNoiseLite detailGenerator = new FastNoiseLite();

	[Signal]
	public delegate void ObstacleGeneratedEventHandler(double width, double height);
	[Signal]
	public delegate void UsableCreatedEventHandler(Tile tile, Vector2I MapCoord);
	[Signal]
	public delegate void ObstacleSoledEventHandler(Vector2I position, Tile tile);
	[Signal]
	public delegate void GenerationFinishedEventHandler();
	[Signal]
	public delegate void NewTileEventHandler(Tile tile, Vector2I MapCoord);
	[Signal]
	public delegate void MapChangedEventHandler(Vector2I pos, int layer, Tile tile);
	[Signal]
	public delegate void LightPlacedEventHandler(Vector2 pos);
	[Signal]
	public delegate void LightSoldEventHandler(Vector2 pos);
	[Signal]
	public delegate void SellAnimalEventHandler();


	[Export] public TileMapLayer Layer0; //Ground, water
	[Export] public TileMapLayer Layer1; //Flowers, etc
	[Export] public LightManager Light; //Light for the map

	[Export] public TileMapLayer PreviewLayer; //Preview for Buy Menu

	[Export] public Label EntranceText;
	[Export] public Label ExitText;

	[Export] public int width = 200;
	[Export] public int height = 200;

	[Export] public int ParkingLotWidth = 10;

	public int Width { get { return width; } }
	public int Height { get { return height; } }
	

	private BuyMenuOverlay buyMenuOverlay;
	private Tile selectedTile;
	private Vector2I prevMousePosition;
	private MapData mapData;
	public RoadGraph roadGraph;
	public ParkingLot parkingLot;

	private List<Label> _roadLabels = new List<Label>();
	public MapData GetMapData() => mapData;

	public override void _Ready()
	{
		mapData = new MapData(Width, Height, 2);
		roadGraph = new RoadGraph(Width, Height);
		Random random = new Random();

		terrainGenerator.Seed = random.Next();
		terrainGenerator.FractalType = FastNoiseLite.FractalTypeEnum.Ridged;
		terrainGenerator.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;

		detailGenerator.Seed = random.Next();
		detailGenerator.NoiseType = FastNoiseLite.NoiseTypeEnum.ValueCubic;
		detailGenerator.FractalType = FastNoiseLite.FractalTypeEnum.PingPong;
		detailGenerator.Frequency = 0.3F;

		Light = GetNode<LightManager>("MainLight");
		buyMenuOverlay = GetNode<BuyMenuOverlay>("/root/MainModel/GameOverlay/Control/TabContainer");
		buyMenuOverlay.PlaceTile += OnPlaceTile;
		buyMenuOverlay.SelectedTileChanged += OnSelectedTileChanged;

		EntranceText.Position = Layer0.MapToLocal(roadGraph.Entrance - new Vector2I(3, 0));
		ExitText.Position = Layer0.MapToLocal(roadGraph.Exit);

		parkingLot = new ParkingLot(roadGraph.Entrance, ParkingLotWidth);
		//GenerateMap();
	}


	public override void _Process(double delta)
	{
		if (selectedTile != null)
		{
			PreviewLayer.SetCell(prevMousePosition, -1, new Vector2I(-1, -1)); //Clear previous tile
			Vector2I position = PreviewLayer.LocalToMap(GetLocalMousePosition());
			prevMousePosition = position;
			if (selectedTile is RoadTile roadTile)
				selectedTile = CheckRoad(position, roadTile);
			PreviewLayer.SetCell(position, selectedTile.AtlasId, selectedTile.AtlasCoord);
			PreviewLayer.ZIndex = selectedTile.Layer;
			if (mapData.CanPlace(position, selectedTile))
				PreviewLayer.Modulate = new Color(1f, 1f, 1f, 0.5f);
			else
				PreviewLayer.Modulate = new Color(1f, 0f, 0f, 0.5f);
		}
	}

	public void GenerateMap()
	{
		Random random = new Random();
		for (int i = 0; i < width; ++i)
		{
			for (int j = 0; j < height; ++j)
			{
				Vector2I tilePosition = new Vector2I(i, j);

				double terrainVal = terrainGenerator.GetNoise2D(i, j);
				double detailVal = detailGenerator.GetNoise2D(i, j);
				const double detailTreshold = -0.8;
				if (terrainVal > -0.2)
				{
					ChangeTile(tilePosition, new DirtTile());
					if (detailVal < detailTreshold)
						ChangeTile(tilePosition, new RockTile(false));
				}
				else if (terrainVal < -0.65)
				{
					ChangeTile(tilePosition, new WaterTile());
				}
				else
				{
					ChangeTile(tilePosition, new GrassTile());
					if (detailVal < detailTreshold)
					{
						if (random.NextDouble() < 0.8)
						{
							ChangeTile(tilePosition, new FlowerTile(false));
						}
						else
						{
							ChangeTile(tilePosition, new LogTile(false));
						}
					}
				}
			}
		}
		GenerateDefaultRoad();
		DrawParkingLot();
		EmitSignal(nameof(GenerationFinished));
	}

	private void GenerateDefaultRoad()
	{
		var path = roadGraph.GenerateDefaultRoad(mapData);
		if (path.Count == 0)
			return;

		foreach (Vector2I pos in path)
		{
			ChangeTile(pos, new EmptyTile(), 1);
			ChangeTile(pos, new RoadTile());
		}
		foreach (Vector2I pos in path)
			CheckRoads(pos);
	}
	public bool IsInBounds()
	{
		Vector2I mapPos = Layer1.LocalToMap(GetLocalMousePosition());
		return mapData.IsInBounds(mapPos, 1);
	}

	private void ChangeTile(Vector2I position, Tile tile, int layer = -1)
	{
		if (layer == -1) layer = tile.Layer;

		mapData.SetTile(position, layer, tile);
		if (layer == 0)
			Layer0.SetCell(position, tile.AtlasId, tile.AtlasCoord);
		else if (layer == 1)
			Layer1.SetCell(position, tile.AtlasId, tile.AtlasCoord);

		if (!tile.IsPassable)
		{
			EmitSignal(nameof(UsableCreated), tile, position);
			EmitSignal(nameof(ObstacleGenerated), position[0], position[1]);
		}

		EmitSignal(SignalName.MapChanged, position, layer, tile);
	}

	private void SellTile(Vector2I pos, int layer)
	{
		if (GameVariables.Instance.IsGameOver)
			return;
		if (!mapData.IsInBounds(pos, layer)) return;
		Tile tile = mapData.GetTile(pos, layer);
		Buyable buyable = tile as Buyable;
		GameVariables.Instance.AddMoney(buyable.Price);
		if (layer == 1)
			ChangeTile(pos, new EmptyTile(), 1);
		else
		{
			ChangeTile(pos, new DirtTile(), 0);
			CheckRoads(pos);
		}

		if (tile.EmitsLight)
		{
			Vector2 p = Layer0.MapToLocal(pos);
			EmitSignal(nameof(LightSold), p);
		}

		if (!tile.IsPassable)
		{

			EmitSignal(nameof(ObstacleSoled), pos, tile);
		}
	}
	public void RemoveTile(Vector2I Mapcoord)
	{
	   
			
			
			
			 ChangeTile(Mapcoord, new EmptyTile(), 1);
		
	}

	private void OnPlaceTile(Tile tile)
	{
		if (GameVariables.Instance.IsGameOver)
			return;
		Vector2I position = Layer1.LocalToMap(GetLocalMousePosition());
		if (tile is Buyable buyable
			&& GameVariables.Instance.HasEnoughMoney(buyable.Price)
			&& mapData.CanPlace(position, tile))
		{
			ChangeTile(position, tile);
			if (tile is RoadTile)
			{
				roadGraph.AddRoadCell(position);
				CheckRoads(position);
			}
			GameVariables.Instance.DecreaseMoney(buyable.Price);
			if (tile.EmitsLight)
			{
				Vector2 p = Layer0.MapToLocal(position);
				EmitSignal(nameof(LightPlaced), p);
			}

			EmitSignal(SignalName.NewTile, tile, position);

		}

		if (tile is EmptyTile emptyTile && mapData.IsInBounds(position, 0))
		{
			if (mapData.GetTile(position, 0) is RoadTile)
			{
				roadGraph.RemoveRoadCell(position);
			}
			if (mapData.GetTile(position, 1) is Buyable)
				SellTile(position, 1);
			else if (mapData.GetTile(position, 0) is Buyable)
				SellTile(position, 0);
			else{
				EmitSignal(SignalName.SellAnimal);
			}
		}
	}

	public void OnSelectedTileChanged(Tile type)
	{
		selectedTile = type;
	}

	private bool IsRoadAt(Vector2I pos)
	{
		if (mapData.IsInBounds(pos, 0))
			return mapData.GetTile(pos, 0) is RoadTile;
		return false;
	}
	private RoadTile CheckRoad(Vector2I pos, RoadTile roadTile)
	{
		bool hasNW = IsRoadAt(pos + new Vector2I(-1, 0));
		bool hasNE = IsRoadAt(pos + new Vector2I(0, -1));
		bool hasSE = IsRoadAt(pos + new Vector2I(1, 0));
		bool hasSW = IsRoadAt(pos + new Vector2I(0, 1));
		if (pos == roadGraph.Entrance) hasNW = true;
		roadTile.SetNeighbors(hasNW, hasNE, hasSW, hasSE);
		return roadTile;
	}
	private void UpdateRoadConectivity(Vector2I pos)
	{
		Tile tile = mapData.GetTile(pos, 0);
		if (tile is RoadTile roadTile)
			ChangeTile(pos, CheckRoad(pos, roadTile));
	}
	private void CheckRoads(Vector2I pos)
	{
		UpdateRoadConectivity(pos + new Vector2I(-1, 0));
		UpdateRoadConectivity(pos + new Vector2I(0, -1));
		UpdateRoadConectivity(pos + new Vector2I(1, 0));
		UpdateRoadConectivity(pos + new Vector2I(0, 1));
		UpdateRoadConectivity(pos + new Vector2I(0, 0));
	}

	private void DrawParkingLot()
	{
		RoadTile road = new RoadTile();
		foreach (var p in parkingLot.Slots)
		{
			road.SetNeighbors(false, false, false, true);
			Layer0.SetCell(p.GridPosition, road.AtlasId, road.AtlasCoord);
		}
		Vector2I ent = roadGraph.Entrance;
		for (int y = 1; y < ParkingLotWidth; y++)
		{
			Vector2I pos = ent + new Vector2I(-2, y);
			road.SetNeighbors(true, true, true, false);
			Layer0.SetCell(pos, road.AtlasId, road.AtlasCoord);
			roadGraph.AddRoadCell(pos);
		}
		for (int y = 1; y < ParkingLotWidth; y++)
		{
			Vector2I pos = ent + new Vector2I(-2, -y);
			road.SetNeighbors(true, true, true, false);
			Layer0.SetCell(pos, road.AtlasId, road.AtlasCoord);
			roadGraph.AddRoadCell(pos);
		}
		road.SetNeighbors(true, false, true, false);
		Layer0.SetCell(ent + new Vector2I(-2, -ParkingLotWidth), road.AtlasId, road.AtlasCoord);
		roadGraph.AddRoadCell(ent + new Vector2I(-2, -ParkingLotWidth));
		road.SetNeighbors(true, true, false, false);
		Layer0.SetCell(ent + new Vector2I(-2, ParkingLotWidth), road.AtlasId, road.AtlasCoord);
		roadGraph.AddRoadCell(ent + new Vector2I(-2, ParkingLotWidth));
		road.SetNeighbors(true, true, true, true);
		Layer0.SetCell(ent + new Vector2I(-2, 0), road.AtlasId, road.AtlasCoord);
		roadGraph.AddRoadCell(ent + new Vector2I(-2, 0));
		road.SetNeighbors(true, false, false, true);
		Layer0.SetCell(ent + new Vector2I(-1, 0), road.AtlasId, road.AtlasCoord);
		roadGraph.AddRoadCell(ent + new Vector2I(-1, 0));
	}
	//-------------------Save/Load-------------------
	public List<TileSave> SaveMap()
	{
		List<TileSave> tiles = new List<TileSave>();
		for (int i = 0; i < Width; ++i)
		{
			for (int j = 0; j < Height; ++j)
			{
				Vector2I tilePosition = new Vector2I(i, j);
				Tile tile = mapData.GetTile(tilePosition, 0);
				if (tile != null)
				{
					TileSave tileSave = new TileSave(tilePosition, tile, 0);
					tiles.Add(tileSave);
				}
				if (mapData.GetTile(tilePosition, 1) != null)
				{
					Tile tile1 = mapData.GetTile(tilePosition, 1);
					TileSave tileSave = new TileSave(tilePosition, tile1, 1);
					tiles.Add(tileSave);
				}
			}
		}
		return tiles;
	}
	public void LoadMap(List<TileSave> tiles)
	{
		foreach (TileSave tile in tiles)
		{
			
			Vector2I pos = new Vector2I(tile.X, tile.Y);
			Tile newTile = TileFactory.CreateTile(tile.TileType);
			
			if (tile.Layer == 0)
			{
				ChangeTile(pos, newTile);
				if (newTile is RoadTile)
				{
					roadGraph.AddRoadCell(pos);
					CheckRoads(pos);
				}
			}
			else
			{
				ChangeTile(pos, newTile, 1);
			}
		}
		DrawParkingLot();
		EmitSignal(nameof(GenerationFinished));
	}
	public LightSave SaveLight()
	{
		return Light.Save();
	}
	public void LoadLight(LightSave save)
	{
		Light.Load(save);
	}
}
