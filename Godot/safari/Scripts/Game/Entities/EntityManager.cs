using Godot;
using System;
using System.Collections.Generic;
using Safari.Scripts.Game.Entities.Animals.UsableTiles;
using Safari.Scripts.Game.Tiles;
using System.Threading.Tasks;
using System.Linq;
using Safari.Scripts.Game.Road;
using Safari.Scripts.Game.Entities;

public partial class EntityManager : Node2D
{
	private NavigationRegion2D _navRegion;
	private MapManager _mapManager;
	private int counter;
	private long  AnimalId;
	private Random _random;

	[Export] public NodePath MapManagerPath;
	[Export] public NodePath BuyMenuPath;
	[Export] public PackedScene StagScene;
	[Export] public PackedScene WolfScene;
	[Export] public PackedScene BoarScene;
	[Export] public PackedScene HyenaScene;
	[Export] public PackedScene JeepScene;
	public List<Stag> Stags => _stags;
	private List<Stag> _stags = new();
	public List<Wolf> Wolves => _wolves;
	private List<Wolf> _wolves = new();

	private List<Boar> _boars = new();
	public List<Boar> Boars => _boars;
	private List<Hyena> _hyenas = new();
	public List<Hyena> Hyenas => _hyenas;

	private List<UsableTile> UsableTiles = new();
	private List<Animal>Corpses = new();
	private List<Vector2> Holes = new();

	private Random _rng = new Random();

	private Vector2 TopLeft;
	private Vector2 BottomRight;
	private Vector2 BottomLeft;
	private Vector2 TopRight;

	private bool IsGenerationFinished = false;
	private bool _isBaking = false;
	private NavigationPolygon MeshWithHoles;
	private NavigationMeshSourceGeometryData2D GeoMesh = new NavigationMeshSourceGeometryData2D();

	private BuyMenuOverlay _buyMenu;
	private List<Jeep> _jeeps;
	private List<Tourist> _tourists;
	private float _reviewAverage;
	private int _reviewCount;

	public MapManager GetMapManager() => _mapManager;
	public override void _Ready()
	{
		_navRegion = GetNode<NavigationRegion2D>("NavigationRegion2D");
		_mapManager = GetNode<MapManager>(MapManagerPath);
		_buyMenu = GetNode<BuyMenuOverlay>(BuyMenuPath);

		_buyMenu.JeepBought += OnJeepBought;
		_jeeps = [];
		_tourists = [];
		_reviewAverage = 3f;
		_reviewCount = 1;
		AnimalId = 1;
		counter = 0;
		_random = new Random();
		var newNavigationMesh = new NavigationPolygon();

		TopLeft = _mapManager.ToGlobal(_mapManager.Layer1.MapToLocal(new Vector2I(-1, -1)));
		BottomRight = _mapManager.ToGlobal(_mapManager.Layer1.MapToLocal(new Vector2I(_mapManager.Width, _mapManager.Height)));
		BottomLeft = _mapManager.ToGlobal(_mapManager.Layer1.MapToLocal(new Vector2I(0, _mapManager.Height)));
		TopRight = _mapManager.ToGlobal(_mapManager.Layer1.MapToLocal(new Vector2I(_mapManager.Width, 0)));


		Vector2[] corners = { TopLeft, BottomLeft, BottomRight, TopRight };
		newNavigationMesh.AddOutline(corners);

		NavigationServer2D.BakeFromSourceGeometryData(newNavigationMesh, new NavigationMeshSourceGeometryData2D());
		_navRegion.NavigationPolygon = newNavigationMesh;

		if (_mapManager != null)
		{
			_mapManager.ObstacleGenerated += OnObstacleGenerated;
			_mapManager.UsableCreated += OnUsableCreated;
			_mapManager.GenerationFinished += RebuildNavMeshWithObstacles;
			_mapManager.NewTile += OnNewTileByPlayer;
			_mapManager.ObstacleSoled += OnSoledObstacle;
			//NewGame();
		}
		else
		{
			GD.PrintErr("MapManager not assigned in the Inspector!");
		}
	}
	public void NewGame()
	{
		_mapManager.GenerateMap();
		for (int i = 0; i < 5; i++)
		{

			SpawnAnimal(new Vector2(200 + i * 100, 400), new Wolf());
			SpawnAnimal(new Vector2(200 + i * 100, 600), new Hyena());
			SpawnAnimal(new Vector2(200 + i * 100, 800), new Stag());
			SpawnAnimal(new Vector2(200 + i * 100, 1000), new Boar());

		}
	}
	private void OnJeepBought()
	{
		var jeep = (Jeep)JeepScene.Instantiate();
		int price = (jeep as Buyable).Price;
		if (!GameVariables.Instance.DecreaseMoney(price))
			return;

		JeepParkingSlot slot = _mapManager.parkingLot.GetFreeSlot();
		if (slot is null) return;

		jeep.Init(slot, _mapManager);
		AddChild(jeep);
		_jeeps.Add(jeep);
	}

	private void OnLeaveReview(float review)
	{
		float sum = _reviewAverage * _reviewCount + review;
		_reviewCount++;
		_reviewAverage = sum / _reviewCount;
		//GD.Print(_reviewAverage);
	}

	//-------------------ANIMAL FUNCTIONS-------------------
	


	private void SpawnTourist()
	{
		if(_rng.NextDouble() < .0005 )
		{
			int spawnCount = Mathf.FloorToInt(Mathf.Pow(1.75f, _reviewAverage));
			for (int i = 0; i < spawnCount; ++i)
			{
				Tourist t = new Tourist();
				t.Review += OnLeaveReview;
				_tourists.Add(t);
			}
			//GD.PrintS("Tourist spawned: ", _tourists.Count);
		}
	}

	private void StartJeep()
	{
		Jeep jeep = _jeeps.FirstOrDefault(j => j.Available);
		if (jeep == null || _tourists.Count == 0) return;

		List<Tourist> passengers = [];
		for (int i = 0; i<Math.Min(jeep.Capacity, _tourists.Count); ++i)
		{
			_tourists[0].BuyTicet();
			passengers.Add(_tourists[0]);
			_tourists.Remove(_tourists[0]);
		}
		jeep.LoadTourists(passengers);
	}

	

	private void OnAnimalWillBeDeleted(Animal animal)
	{
		animal.Water.Clear();
		animal.CurrentWaterToGo = null;
		if (animal is Carnivore)
		{

			((Carnivore)animal).Food.Clear();
			((Carnivore)animal).CurrentThingToEat = null;

		}
		else
		{
			((Herbivore)animal).Food.Clear();
			((Herbivore)animal).CurrentVegetationToEat = null;
		}

		foreach(Wolf wolf in _wolves)
		{
			if (wolf.Food.Contains(animal))
			{
				wolf.Food.Remove(animal);
			}
			if (wolf.CurrentThingToEat == animal)
			{
				wolf.CurrentThingToEat = null;
			}
		}

		foreach (Hyena hyena in _hyenas)
		{
			if (hyena.Food.Contains(animal))
			{
				hyena.Food.Remove(animal);
			}
			if (hyena.CurrentThingToEat == animal)
			{
				hyena.CurrentThingToEat = null;
			}
		}

		Corpses.Remove(animal);
		/*theres a bug when a game is loaded in instead of a newgame so that dead animal can appear in the living list 
		 it doesnt mean anything from gameplay wise but not Disposing it will crash the game*/
		if (animal is Stag)
		{
			_stags.Remove((Stag)animal);
		}
		else if (animal is Wolf)
		{
			_wolves.Remove((Wolf)animal);
		}
		else if (animal is Boar)
		{
			_boars.Remove((Boar)animal);
		}
		else if (animal is Hyena)
		{
			_hyenas.Remove((Hyena)animal);
		}


	}
	private void OnAnimalSee(Animal animal)
	{

		foreach (UsableTile tile in UsableTiles)
		{




			if (tile.TileType is WaterTile)
			{
				if (animal.GlobalPosition.DistanceTo(tile.WorldCoords) < 500)
				{
					if (!animal.Water.Contains(tile))
						animal.Water.Add(tile);
				}
			}

		}
		if (animal is Carnivore)
		{
			foreach (Animal corpse in Corpses)
			{


				if (((Carnivore)animal).GlobalPosition.DistanceTo(corpse.GlobalPosition) < 1000)
				{
					if (!((Carnivore)animal).Food.Contains(corpse))
						((Carnivore)animal).Food.Add(corpse);
				}

			}
		}
		else
		{
			foreach(UsableTile tile in UsableTiles)
			{
				if (tile.TileType is FlowerTile || tile.TileType is TreeTile)
				{
					if (animal.GlobalPosition.DistanceTo(tile.WorldCoords) < 500)
					{
						if (!((Herbivore)animal).Food.Contains(tile))
							((Herbivore)animal).Food.Add(tile);
					}
				}
			}
		}
		
	}
	
	private void  OnThirstyAnimal(Animal animal)
	{
		if (animal.Water.Count == 0)
		{

			animal.GoDrink(null,null);
		}
		else
		{
			
			UsableTile target = animal.Water.MinBy(x => animal.GlobalPosition.DistanceTo(x.WorldCoords));

			
			Vector2 targetCoord = target.WorldCoords + new Vector2(_rng.Next(-50, 50), _rng.Next(-50, 50));
			animal.GoDrink(targetCoord, target);

		}
	}
	private void OnAnimalHungry(Animal animal)
	{
		if(animal is Carnivore)
		{
			if (((Carnivore)animal).Food.Count == 0)
			{
				GD.Print($"wolf hungry {((Carnivore)animal).GlobalPosition} no food");
				List<Herbivore> herbivores = _stags.Cast<Herbivore>().Concat(_boars.Cast<Herbivore>()).ToList();
				foreach (Herbivore herbivore in herbivores)
				{
					if (((Carnivore)animal).GlobalPosition.DistanceTo(herbivore.GlobalPosition) < 500)
					{
						
						((Carnivore)animal).GoHunt(herbivore.GlobalPosition, herbivore);
						return;
					}
				}
				herbivores.Clear();
				((Carnivore)animal).GoHunt(new Vector2(0, 0), null);
			}
			else
			{
				Animal TargetCorpse = ((Carnivore)animal).Food.MinBy(x => ((Carnivore)animal).GlobalPosition.DistanceTo(x.GlobalPosition));
				Vector2 target = TargetCorpse.GlobalPosition;

				
				((Carnivore)animal).GoHunt(target, TargetCorpse);
			}
		}
		else
		{
			if (((Herbivore)animal).Food.Count == 0)
			{
				GD.Print($"herbivore hungry {((Herbivore)animal).GlobalPosition} no food");
				((Herbivore)animal).GoGrazing(null, null);

			}
			else
			{
				UsableTile target = ((Herbivore)animal).Food.MinBy(x => animal.GlobalPosition.DistanceTo(x.WorldCoords));
				Vector2 targetCood = target.WorldCoords + new Vector2(_rng.Next(-50, 50), _rng.Next(-50, 50));
				((Herbivore)animal).GoGrazing(targetCood, target);

			}

		}
		

		
	   

	}
	private void OnAnimalDied(Animal animal)
	{
		switch (animal)
		{
			case Stag stag:
				_stags.Remove(stag);
				break;
			case Wolf wolf:
				_wolves.Remove(wolf);
				break;
			case Boar boar:
				_boars.Remove(boar);
				break;
			case Hyena hyena:
				_hyenas.Remove(hyena);
				break;
			default:
				GD.PrintErr("Unknown animal type");
				break;
		}
		Corpses.Add(animal);
	}
	private void SpawnAnimal(Vector2 position,Animal animal){
		Animal SpawnedAnimal;

		switch (animal)
		{
			case Stag stag:
				SpawnedAnimal = (Stag)StagScene.Instantiate();
				_stags.Add((Stag)SpawnedAnimal);
				break;
			case Wolf wolf:
				SpawnedAnimal = (Wolf)WolfScene.Instantiate();
				_wolves.Add((Wolf)SpawnedAnimal);
				break;
			case Boar boar:
				SpawnedAnimal = (Boar)BoarScene.Instantiate();
				_boars.Add((Boar)SpawnedAnimal);
				break;
			case Hyena hyena:
				SpawnedAnimal = (Hyena)HyenaScene.Instantiate();
				_hyenas.Add((Hyena)SpawnedAnimal);
				break;
			default:
				
				return;

		}
		SpawnedAnimal.Id = AnimalId++;
		SpawnedAnimal.GlobalPosition = position;
		SpawnedAnimal.AnimalThirsty += OnThirstyAnimal;
		SpawnedAnimal.AnimalHungry += OnAnimalHungry;
		SpawnedAnimal.AnimalDied += OnAnimalDied;
		SpawnedAnimal.AnimalSee += OnAnimalSee;
		SpawnedAnimal.AnimalWillBeDeleted += OnAnimalWillBeDeleted;
		AddChild(SpawnedAnimal);
		SpawnedAnimal.StateMachine.Start();
	}
	private void GroupUpAllAnimals()
	{
		Vector2 CurrentGroupUp;
		CurrentGroupUp = CreateValidCoord();

		foreach (Stag stag in _stags)
		{
			if (stag.StateMachine.CurrentState is IdleState)
			{
				stag.MoveTo(CurrentGroupUp + new Vector2(_rng.Next(-100, 100), _rng.Next(-100, 100)));
				stag.StateMachine.ChangeState("GroupUpState");
				
			}
		}
		CurrentGroupUp = CreateValidCoord();

		foreach (Wolf wolf in _wolves)
		{
			if (wolf.StateMachine.CurrentState is IdleState)
			{
				wolf.MoveTo(CurrentGroupUp + new Vector2(_rng.Next(-100, 100), _rng.Next(-100, 100)));
				wolf.StateMachine.ChangeState("GroupUpState");
				
			}
		}
		CurrentGroupUp = CreateValidCoord();
		foreach (Hyena hyena in _hyenas)
		{
			if (hyena.StateMachine.CurrentState is IdleState)
			{
				hyena.MoveTo(CurrentGroupUp + new Vector2(_rng.Next(-100, 100), _rng.Next(-100, 100)));
				hyena.StateMachine.ChangeState("GroupUpState");
				
			}
		}
		CurrentGroupUp = CreateValidCoord();
		foreach (Boar boar in _boars)
		{
			if (boar.StateMachine.CurrentState is IdleState)
			{
				boar.MoveTo(CurrentGroupUp + new Vector2(_rng.Next(-100, 100), _rng.Next(-100, 100)));
				boar.StateMachine.ChangeState("GroupUpState");
				
			}
		}
	}
	private void ReproduceAllAnimal()
	{
		foreach (Boar boar in _boars)
		{
			if (boar.StateMachine.CurrentState is IdleState && boar.StateMachine.CurrentState.Age > 4)
			{

				foreach (Boar otherBoar in _boars)

				{
					if (boar != otherBoar && otherBoar.StateMachine.CurrentState is IdleState && otherBoar.StateMachine.CurrentState.Age > 4)
					{
						if (boar.GlobalPosition.DistanceTo(otherBoar.GlobalPosition) < 200)
						{
							GD.Print("New boar created");

						   SpawnAnimal(boar.GlobalPosition + new Vector2(_rng.Next(-20, 20), _rng.Next(-20, 20)), new Boar());
							goto Boar_EndLoop;
						}
					}
				}
			}
		}
	Boar_EndLoop:;

		foreach (Stag stag in _stags)
		{
			if (stag.StateMachine.CurrentState is IdleState && stag.StateMachine.CurrentState.Age > 4)
			{
				foreach (Stag OtherStag in _stags)
				{
					if (stag != OtherStag && OtherStag.StateMachine.CurrentState is IdleState && OtherStag.StateMachine.CurrentState.Age > 4)
					{
						if (stag.GlobalPosition.DistanceTo(OtherStag.GlobalPosition) < 200)
						{
							GD.Print("New stag created");

							SpawnAnimal(stag.GlobalPosition + new Vector2(_rng.Next(-20, 20), _rng.Next(-20, 20)), new Stag());
							goto EndLoop;
						}
					}
				}

			}
		}
	EndLoop:;
		foreach (Wolf wolf in _wolves)
		{
			if (wolf.StateMachine.CurrentState is IdleState && wolf.StateMachine.CurrentState.Age > 4)
			{
				foreach (Wolf OtherWolf in _wolves)
				{
					if (wolf != OtherWolf && OtherWolf.StateMachine.CurrentState is IdleState && OtherWolf.StateMachine.CurrentState.Age > 4)
					{
						if (wolf.GlobalPosition.DistanceTo(OtherWolf.GlobalPosition) < 2000)
						{
							GD.Print("New wolf created");
							SpawnAnimal(wolf.GlobalPosition + new Vector2(_rng.Next(-20, 20), _rng.Next(-20, 20)), new Wolf());
							goto Wolf_EndLoop;
						}
					}
				}

			}
		}
	Wolf_EndLoop:;
		foreach (Hyena hyena in _hyenas)
		{
			if (hyena.StateMachine.CurrentState is IdleState && hyena.StateMachine.CurrentState.Age > 4)
			{
				foreach (Hyena otherHyena in _hyenas)
				{
					if (hyena != otherHyena &&
						otherHyena.StateMachine.CurrentState is IdleState &&
						otherHyena.StateMachine.CurrentState.Age > 4)
					{
						if (hyena.GlobalPosition.DistanceTo(otherHyena.GlobalPosition) < 2000)
						{
							GD.Print("New hyena created");
							SpawnAnimal(hyena.GlobalPosition + new Vector2(_rng.Next(-20, 20), _rng.Next(-20, 20)), new Hyena());
							goto Hyena_EndLoop;
						}
					}
				}
			}
		}
	Hyena_EndLoop:;
	}
	private void RemoveTileFromAnimals(Vector2I Coords,Tile tile)
	{
		

		UsableTiles.RemoveAll(x => x.MapCoords.X == Coords.X && x.MapCoords.Y == Coords.Y);
		if ((tile is FlowerTile || tile is TreeTile))
		{
			List<Herbivore> herbivores = _stags.Cast<Herbivore>().Concat(_boars.Cast<Herbivore>()).ToList();
			foreach (Herbivore animal in herbivores)

			{
				animal.Food.RemoveAll(x => x.MapCoords.X == Coords.X && x.MapCoords.Y == Coords.Y);

				if (animal.CurrentWaterToGo != null &&  (animal).CurrentVegetationToEat.MapCoords == Coords)
				{
					(animal).CurrentVegetationToEat = null;

				}
			}
			herbivores.Clear();
		}
		else if(tile is WaterTile)
		{
			List<Animal> AllAnimal = _stags.Cast<Animal>().Concat(_wolves).Concat(_boars).Concat(_hyenas).ToList();
			foreach(Animal animal in AllAnimal)
			{
				if(animal.Water.RemoveAll(x => x.MapCoords.X == Coords.X && x.MapCoords.Y == Coords.Y)> 0)
				{
					GD.PrintErr("Water removed");
				}
				if (animal.CurrentWaterToGo != null && animal.CurrentWaterToGo.MapCoords == Coords)
				{
					animal.CurrentWaterToGo = null;
				}

			}
		}
	}


	//-------------------FOR NAVIGATION-------------------
	private void OnUsableCreated(Tile tile, Vector2I MapCoord)

	{
		Vector2 worldPosition = _mapManager.ToGlobal(_mapManager.Layer1.MapToLocal(MapCoord));

		
		

		UsableTile usable = new UsableTile(tile,MapCoord, (Vector2I)worldPosition);
		UsableTiles.Add(usable);
	}

	private void OnObstacleGenerated(double x, double y)
	{
		Vector2I tilePosition = new Vector2I((int)x, (int)y);
		Vector2 worldPosition = _mapManager.Layer1.ToGlobal(_mapManager.Layer1.MapToLocal(tilePosition));

		AddObstacle(worldPosition);
	}

	private void OnSoledObstacle(Vector2I MapCoord,Tile tile)

	{
		Vector2 worldPosition = _mapManager.ToGlobal(_mapManager.Layer1.MapToLocal(MapCoord));
		//GD.PrintErr(MeshWithHoles.GetOutlineCount());
		MeshWithHoles.RemoveOutline(Holes.IndexOf(worldPosition) + 1);
		//GD.PrintErr(Holes.IndexOf(worldPosition));
		//GD.PrintErr(MeshWithHoles.GetOutlineCount());
		Holes.Remove(worldPosition);

		RemoveTileFromAnimals(MapCoord, tile);

		StartBaking();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	private void AddObstacle(Vector2 position)
	{
		if (IsGenerationFinished)
		{
			float halfSize = 2;

			Vector2[] hole = {
			position + new Vector2(halfSize, halfSize),
			position + new Vector2(halfSize, -halfSize),
			position + new Vector2(-halfSize, -halfSize),
			position + new Vector2(-halfSize, halfSize),
			position + new Vector2(halfSize, halfSize)
		};

			if (!Holes.Contains(position))
			{
				Holes.Add(position);
				MeshWithHoles.AddOutline(hole);


			}
			

		}
		else
		{
			Holes.Add(position);
		}
	}

	private void StartBaking()
	{
		NavigationPolygon navMesh = (NavigationPolygon)_navRegion.NavigationPolygon.Duplicate();

		_navRegion.NavigationPolygon = null;
		_navRegion.NavigationPolygon = navMesh;

		Task.Run(() =>
		{
			NavigationServer2D.BakeFromSourceGeometryData(MeshWithHoles, GeoMesh);
			_navRegion.CallDeferred("set_navigation_polygon", MeshWithHoles);
		});
	}

	private void OnNewTileByPlayer(Tile tile, Vector2I MapCoord)
	{
		StartBaking();
	}

	private void RebuildNavMeshWithObstacles()
	{
		IsGenerationFinished = true;
		var newMesh = new NavigationPolygon();

		Vector2[] mainOutline = { TopLeft, TopRight, BottomRight, BottomLeft };
		newMesh.AddOutline(mainOutline);

		foreach (var pos in Holes)
		{
			float halfSize = 2;

			Vector2[] hole = {
				pos + new Vector2(halfSize, halfSize),
				pos + new Vector2(halfSize, -halfSize),
				pos + new Vector2(-halfSize, -halfSize),
				pos + new Vector2(-halfSize, halfSize),
				pos + new Vector2(halfSize, halfSize)
			};

			newMesh.AddOutline(hole);
		}

		MeshWithHoles = newMesh;
		NavigationServer2D.BakeFromSourceGeometryData(newMesh, GeoMesh);
		_navRegion.NavigationPolygon = newMesh;
	}

	private Vector2 CreateValidCoord()
	{
		int x = _random.Next(2700) - 785;
		int y = _random.Next(2700) + 358;
		
		while (Math.Pow(x-565,2) + Math.Pow(y-1708,2) > 1822500)
		{
			y = _random.Next(2700) + 358;
		}
		return new Vector2(x,y);
	}
	//-------------------SAVE/LOAD-------------------
	
	public List<UsableTileSave> SaveUsableTiles()
	{
		List<UsableTileSave> saves = new List<UsableTileSave>();
		foreach (var usable in UsableTiles)
		{
			saves.Add(new UsableTileSave(usable));
		}
		return saves;
	}
	public void LoadUsableTiles(List<UsableTileSave> saves)
	{
		UsableTiles.Clear();
		foreach (var save in saves)
		{
			Tile tile = TileFactory.CreateTile(save.TileTypeName);
			UsableTile usable = new UsableTile(tile, new Vector2I(save.MapX,save.MapY),new Vector2I(save.WorldX,save.WorldY));
			UsableTiles.Add(usable);
		}
		GD.PrintErr("UsableTiles loaded: " + UsableTiles.Count);
	}
	public List<HerbivoreSave> SaveHerbivores()
	{
		List<HerbivoreSave> saves = new List<HerbivoreSave>();

		foreach (var herb in _stags.Cast<Herbivore>().Concat(_boars))
		{
			HerbivoreSave save = new HerbivoreSave
			{
				AnimalType = herb.GetType().Name,
				PosX = herb.GlobalPosition.X,
				PosY = herb.GlobalPosition.Y,
				Id = herb.Id,
				CurrentStateName = herb.StateMachine.CurrentState.Name,
				Hunger = herb.StateMachine.CurrentState.Hunger,
				Thirst = herb.StateMachine.CurrentState.Thirst,
				Age = herb.StateMachine.CurrentState.Age
			};

			// Save WaterTiles
			save.WaterTileXs = herb.Water.Select(w => w.MapCoords.X).ToList();
			save.WaterTileYs = herb.Water.Select(w => w.MapCoords.Y).ToList();

			// Save FoodTiles
			save.FoodTileXs = herb.Food.Select(f => f.MapCoords.X).ToList();
			save.FoodTileYs = herb.Food.Select(f => f.MapCoords.Y).ToList();
			

			// Save Current Targets
			if (herb.CurrentWaterToGo != null)
			{
				save.CurrentWaterX = herb.CurrentWaterToGo.MapCoords.X;
				save.CurrentWaterY = herb.CurrentWaterToGo.MapCoords.Y;
			}
			if (herb.GroupUpCoord != null)
			{
				save.CurrentGroupUpX = ((Vector2)herb.GroupUpCoord).X;
				save.CurrentGroupUpY = ((Vector2)herb.GroupUpCoord).Y;
			}

			if (herb.CurrentVegetationToEat != null)
			{
				save.CurrentVegetationX = herb.CurrentVegetationToEat.MapCoords.X;
				save.CurrentVegetationY = herb.CurrentVegetationToEat.MapCoords.Y;
			}

			saves.Add(save);
		}

		return saves;
	}


	public void LoadHerbivores(List<HerbivoreSave> herbivoreSaves)
	{
		GD.PrintErr($"Start loading herbivores. Count = {herbivoreSaves.Count}");

		foreach (var save in herbivoreSaves)
		{
			GD.PrintErr($"Loading herbivore of type {save.AnimalType} with ID {save.Id}");

			Animal newAnimal = null;

			// Instantiate the right animal type
			if (save.AnimalType == nameof(Stag))
			{
				if (StagScene == null)
				{
					GD.PrintErr("StagScene is NULL!");
					continue;
				}
				newAnimal = (Stag)StagScene.Instantiate();
				_stags.Add((Stag)newAnimal);
				GD.PrintErr("Instantiated a Stag");
			}
			else if (save.AnimalType == nameof(Boar))
			{
				if (BoarScene == null)
				{
					GD.PrintErr("BoarScene is NULL!");
					continue;
				}
				newAnimal = (Boar)BoarScene.Instantiate();
				_boars.Add((Boar)newAnimal);
				GD.PrintErr("Instantiated a Boar");
			}
			else
			{
				GD.PrintErr($"Unknown herbivore type: {save.AnimalType}");
				continue;
			}

			if (newAnimal == null)
			{
				GD.PrintErr("newAnimal is NULL after instantiation!");
				continue;
			}

			// Restore basic properties
			newAnimal.GlobalPosition = new Vector2(save.PosX, save.PosY);
			newAnimal.Id = save.Id;
			GD.PrintErr($"Set GlobalPosition ({save.PosX},{save.PosY}) and Id {save.Id}");

			// Connect signals
			newAnimal.AnimalThirsty += OnThirstyAnimal;
			newAnimal.AnimalHungry += OnAnimalHungry;
			newAnimal.AnimalDied += OnAnimalDied;
			newAnimal.AnimalSee += OnAnimalSee;
			newAnimal.AnimalWillBeDeleted += OnAnimalWillBeDeleted;
			GD.PrintErr("Connected signals");
			AddChild(newAnimal);
			// Clear previous Water and Food
			if (newAnimal.Water.Count > 0)
			{
				newAnimal.Water.Clear();
			}
			
			GD.PrintErr("Cleared Water list");
			if (newAnimal is Herbivore herbivore)
			{
				herbivore.Food.Clear();
				GD.PrintErr("Cleared Water and Food lists");
			}

			// Rebuild Water tiles
			if (save.WaterTileXs != null && save.WaterTileYs != null)
			{
				for (int i = 0; i < save.WaterTileXs.Count; i++)
				{
					Vector2I coord = new Vector2I(save.WaterTileXs[i], save.WaterTileYs[i]);
					var usable = UsableTiles.FirstOrDefault(u => u.MapCoords == coord);
					if (usable != null)
					{
						newAnimal.Water.Add(usable);
					}
					else
					{
						GD.PrintErr($"WaterTile not found at {coord}");
					}
				}
				GD.PrintErr("Rebuilt Water tiles");
			}
			else
			{
				GD.PrintErr("WaterTileXs or WaterTileYs is NULL!");
			}

			// Rebuild Food tiles
			if (newAnimal is Herbivore herb)
			{
				if (save.FoodTileXs != null && save.FoodTileYs != null)
				{
					for (int i = 0; i < save.FoodTileXs.Count; i++)
					{
						Vector2I coord = new Vector2I(save.FoodTileXs[i], save.FoodTileYs[i]);
						var usable = UsableTiles.FirstOrDefault(u => u.MapCoords == coord);
						if (usable != null)
							herb.Food.Add(usable);
						else
							GD.PrintErr($"FoodTile not found at {coord}");
					}
					GD.PrintErr("Rebuilt Food tiles");
				}
				else
				{
					GD.PrintErr("FoodTileXs or FoodTileYs is NULL!");
				}

				// Restore CurrentVegetationTarget
				if (save.CurrentVegetationX.HasValue && save.CurrentVegetationY.HasValue)
				{
					Vector2I coord = new Vector2I(save.CurrentVegetationX.Value, save.CurrentVegetationY.Value);
					var usable = UsableTiles.FirstOrDefault(u => u.MapCoords == coord);
					if (usable != null)
						herb.CurrentVegetationToEat = usable;
					else
						GD.PrintErr($"CurrentVegetationTarget tile not found at {coord}");
				}
			}

			// Restore CurrentWaterTarget
			if (save.CurrentWaterX.HasValue && save.CurrentWaterY.HasValue)
			{
				Vector2I coord = new Vector2I(save.CurrentWaterX.Value, save.CurrentWaterY.Value);
				var usable = UsableTiles.FirstOrDefault(u => u.MapCoords == coord);
				if (usable != null)
					newAnimal.CurrentWaterToGo = usable;
				else
					GD.PrintErr($"CurrentWaterTarget tile not found at {coord}");
			}

			// Restore GroupUpCoord
			if (save.CurrentGroupUpX.HasValue && save.CurrentGroupUpY.HasValue)
			{
				newAnimal.GroupUpCoord = new Vector2(save.CurrentGroupUpX.Value, save.CurrentGroupUpY.Value);
				GD.PrintErr($"Restored GroupUpCoord to ({save.CurrentGroupUpX},{save.CurrentGroupUpY})");
			}

			// Restore State
			newAnimal.StateMachine.CurrentState = newAnimal.StateMachine.GetStateByName(save.CurrentStateName);
			GD.PrintErr($"Restored state: {save.CurrentStateName}");

			switch (save.CurrentStateName)
			{
				case "HungerState":
					newAnimal.StateMachine.CurrentState.LoadHunger(save.Hunger, save.Thirst, save.Age, save.Count, save.IsHuntGoingOn);
					break;
				case "DeathState":
					newAnimal.StateMachine.CurrentState.LoadDeath(save.Count);
					break;
				case "IdleState":
					newAnimal.StateMachine.CurrentState.LoadIdle();
					break;
				case "ThirstState":
					newAnimal.StateMachine.CurrentState.LoadThirst(save.Count);
					break;
				case "GroupUpState":
					newAnimal.StateMachine.CurrentState.LoadGroupUp(save.Count);
					break;
				default:
					GD.PrintErr($"Unknown state {save.CurrentStateName}");
					break;
			}

			
			GD.PrintErr($"Finished loading herbivore ID {newAnimal.Id}");
		}

		GD.PrintErr("Finished loading all herbivores");
	}
	public List<CorpseSave> SaveCorpses()
	{
		List<CorpseSave> saves = new List<CorpseSave>();

		foreach (var corpse in Corpses)
		{
			if (corpse.StateMachine.CurrentState is DeathState deathState)
			{
				CorpseSave save = new CorpseSave
				{
					AnimalType = corpse.GetType().Name,
					PosX = corpse.GlobalPosition.X,
					PosY = corpse.GlobalPosition.Y,
					Id = corpse.Id,
					DeathCount = deathState.GetCount()
				};
				saves.Add(save);
			}
			else
			{
				GD.PrintErr($"Warning: corpse {corpse.Name} is not in DeathState when saving!");
			}
		}

		return saves;
	}
	public void LoadCorpses(List<CorpseSave> corpseSaves)
	{
		GD.PrintErr($"Start loading corpses. Count = {corpseSaves.Count}");

		foreach (var save in corpseSaves)
		{
			Animal corpse;

			if (save.AnimalType == nameof(Stag))
				corpse = (Stag)StagScene.Instantiate();
			else if (save.AnimalType == nameof(Boar))
				corpse = (Boar)BoarScene.Instantiate();
			else if (save.AnimalType == nameof(Wolf))
				corpse = (Wolf)WolfScene.Instantiate();
			else if (save.AnimalType == nameof(Hyena))
				corpse = (Hyena)HyenaScene.Instantiate();
			else
			{
				GD.PrintErr($"Unknown corpse type: {save.AnimalType}");
				continue;
			}

			AddChild(corpse);
			corpse.GlobalPosition = new Vector2(save.PosX, save.PosY);
			corpse.Id = save.Id;

			corpse.StateMachine.ChangeState("DeathState");

			if (corpse.StateMachine.CurrentState is DeathState deathState)
			{
				deathState.LoadDeath(save.DeathCount);
			}
			else
			{
				GD.PrintErr($"Corpse {corpse.Name} was not correctly put into DeathState after instantiation!");
			}

			Corpses.Add(corpse);

			GD.PrintErr($"Loaded corpse {corpse.Name} at ({save.PosX}, {save.PosY}) with DeathCount {save.DeathCount}");
		}

		GD.PrintErr("Finished loading corpses");
	}

	public List<CarnivoreSave> SaveCarnivores()
	{
		List<CarnivoreSave> saves = new List<CarnivoreSave>();

		foreach (var carn in _wolves.Cast<Carnivore>().Concat(_hyenas))
		{
			CarnivoreSave save = new CarnivoreSave
			{
				AnimalType = carn.GetType().Name,
				PosX = carn.GlobalPosition.X,
				PosY = carn.GlobalPosition.Y,
				Id = carn.Id,
				CurrentStateName = carn.StateMachine.CurrentState.Name,
				Hunger = carn.StateMachine.CurrentState.Hunger,
				Thirst = carn.StateMachine.CurrentState.Thirst,
				Age = carn.StateMachine.CurrentState.Age
				
			};

			// Save Food (as Ids)
			save.FoodIds = carn.Food.Select(f => f.Id).ToList();
			if(carn.StateMachine.CurrentState is HungerState huntState)
			{
				save.IsHuntGoingOn = huntState.IsHuntGoingOn;

			}
			// Save CurrentTarget
			if (carn.CurrentThingToEat != null)
				save.CurrentTargetId = carn.CurrentThingToEat.Id;

			// Save CurrentWaterToGo
			if (carn.CurrentWaterToGo != null)
			{
				save.CurrentWaterX = carn.CurrentWaterToGo.MapCoords.X;
				save.CurrentWaterY = carn.CurrentWaterToGo.MapCoords.Y;
			}

			// Save GroupUpCoord
			if (carn.GroupUpCoord != null)
			{
				save.CurrentGroupUpX = carn.GroupUpCoord.Value.X;
				save.CurrentGroupUpY = carn.GroupUpCoord.Value.Y;
			}

			saves.Add(save);
		}

		return saves;
	}

	public void LoadCarnivores(List<CarnivoreSave> carnivoreSaves)
	{
		GD.PrintErr($"Start loading carnivores. Count = {carnivoreSaves.Count}");

		// Build fast lookup for all animals by ID (for restoring Food and Target)
		Dictionary<long, Animal> allAnimalsById = _stags.Cast<Animal>()
			.Concat(_boars)
			.Concat(Corpses)
			.ToDictionary(a => a.Id, a => a);

		foreach (var save in carnivoreSaves)
		{
			GD.PrintErr($"Loading carnivore of type {save.AnimalType} with ID {save.Id}");

			Carnivore newCarnivore = null;

			if (save.AnimalType == nameof(Wolf))
			{
				if (WolfScene == null)
				{
					GD.PrintErr("WolfScene is NULL!");
					continue;
				}
				newCarnivore = (Wolf)WolfScene.Instantiate();
				_wolves.Add((Wolf)newCarnivore);
				GD.PrintErr("Instantiated a Wolf");
			}
			else if (save.AnimalType == nameof(Hyena))
			{
				if (HyenaScene == null)
				{
					GD.PrintErr("HyenaScene is NULL!");
					continue;
				}
				newCarnivore = (Hyena)HyenaScene.Instantiate();
				_hyenas.Add((Hyena)newCarnivore);
				GD.PrintErr("Instantiated a Hyena");
			}
			else
			{
				GD.PrintErr($"Unknown carnivore type: {save.AnimalType}");
				continue;
			}

			if (newCarnivore == null)
			{
				GD.PrintErr("newCarnivore is NULL after instantiation!");
				continue;
			}

			AddChild(newCarnivore);

			newCarnivore.GlobalPosition = new Vector2(save.PosX, save.PosY);
			newCarnivore.Id = save.Id;
			GD.PrintErr($"Set GlobalPosition ({save.PosX},{save.PosY}) and Id {save.Id}");

			newCarnivore.AnimalThirsty += OnThirstyAnimal;
			newCarnivore.AnimalHungry += OnAnimalHungry;
			newCarnivore.AnimalDied += OnAnimalDied;
			newCarnivore.AnimalSee += OnAnimalSee;
			newCarnivore.AnimalWillBeDeleted += OnAnimalWillBeDeleted;
			GD.PrintErr("Connected signals");

			newCarnivore.Water.Clear();
			newCarnivore.Food.Clear();

			// Rebuild Water
			if (save.WaterTileXs != null && save.WaterTileYs != null)
			{
				for (int i = 0; i < save.WaterTileXs.Count; i++)
				{
					Vector2I coord = new Vector2I(save.WaterTileXs[i], save.WaterTileYs[i]);
					var usable = UsableTiles.FirstOrDefault(u => u.MapCoords == coord);
					if (usable != null)
					{
						newCarnivore.Water.Add(usable);
					}
					else
					{
						GD.PrintErr($"WaterTile not found at {coord}");
					}
				}
			}

			// Rebuild Food
			if (save.FoodIds != null)
			{
				foreach (var id in save.FoodIds)
				{
					if (allAnimalsById.TryGetValue(id, out var prey))
					{
						newCarnivore.Food.Add(prey);
					}
					else
					{
						GD.PrintErr($"Food animal with Id {id} not found!");
					}
				}
			}

			// Restore CurrentThingToEat
			if (save.CurrentTargetId.HasValue)
			{
				if (allAnimalsById.TryGetValue(save.CurrentTargetId.Value, out var target))
				{
					newCarnivore.CurrentThingToEat = target;
					GD.PrintErr($"!!!!Restored CurrentTarget to animal with Id {save.CurrentTargetId.Value}");
				}
				else
				{
					GD.PrintErr($"Current target animal with Id {save.CurrentTargetId.Value} not found!");
				}
			}

			// Restore CurrentWaterTarget
			if (save.CurrentWaterX.HasValue && save.CurrentWaterY.HasValue)
			{
				Vector2I coord = new Vector2I(save.CurrentWaterX.Value, save.CurrentWaterY.Value);
				var usable = UsableTiles.FirstOrDefault(u => u.MapCoords == coord);
				if (usable != null)
					newCarnivore.CurrentWaterToGo = usable;
				else
					GD.PrintErr($"CurrentWaterTarget tile not found at {coord}");
			}

			// Restore GroupUpCoord
			if (save.CurrentGroupUpX.HasValue && save.CurrentGroupUpY.HasValue)
			{
				newCarnivore.GroupUpCoord = new Vector2(save.CurrentGroupUpX.Value, save.CurrentGroupUpY.Value);
				GD.PrintErr($"Restored GroupUpCoord to ({save.CurrentGroupUpX},{save.CurrentGroupUpY})");
			}

			// Restore State
			newCarnivore.StateMachine.CurrentState = newCarnivore.StateMachine.GetStateByName(save.CurrentStateName);
			GD.PrintErr($"Restored state: {save.CurrentStateName}");

			switch (save.CurrentStateName)
			{
				case "HungerState":
					newCarnivore._navAgent.TargetPosition = newCarnivore.CurrentThingToEat.GlobalPosition;
					newCarnivore.StateMachine.CurrentState.LoadHunger(save.Hunger, save.Thirst, save.Age, save.Count, save.IsHuntGoingOn);
					break;
				case "DeathState":
					newCarnivore.StateMachine.CurrentState.LoadDeath(save.Count);
					break;
				case "IdleState":
					newCarnivore.StateMachine.CurrentState.LoadIdle();
					break;
				case "ThirstState":
					newCarnivore.StateMachine.CurrentState.LoadThirst(save.Count);
					break;
				case "GroupUpState":
					newCarnivore.StateMachine.CurrentState.LoadGroupUp(save.Count);
					break;
				default:
					GD.PrintErr($"Unknown state {save.CurrentStateName}");
					break;
			}

			GD.PrintErr($"Finished loading carnivore ID {newCarnivore.Id}");
		}

		GD.PrintErr("Finished loading all carnivores");
	}
	public List<JeepSave> SaveJeeps()
	{
		List<JeepSave> saves = new();

		foreach (var jeep in _jeeps)
		{
			var jeepSave = jeep.Save();
			


			saves.Add(jeepSave);
		}

		return saves;
	}

	public void LoadJeeps(List<JeepSave> jeepSaves)
	{
		foreach (var save in jeepSaves)
		{
			var jeep = (Jeep)JeepScene.Instantiate();
			
			jeep.Load(save, _mapManager, _mapManager.parkingLot.GetFreeSlot());
			JeepParkingSlot slot = _mapManager.parkingLot.GetFreeSlot();
			AddChild(jeep);
			_jeeps.Add(jeep);
		}
	}


	public LightSave SaveLight()
	{
		return _mapManager.SaveLight();
	}
	public void LoadLight(LightSave lightSave)
	{
		_mapManager.LoadLight(lightSave);
	}


	public override void _Process(double delta)
	{
		SpawnTourist();
		StartJeep();

		if (counter == 0)
		{

			GroupUpAllAnimals();


		}
		if (counter % 500 == 0)
			
			{

			ReproduceAllAnimal();
			

		}
		counter++;
			if (counter > 2000)
			{
				counter = 0;

			
		}

	} 
}
