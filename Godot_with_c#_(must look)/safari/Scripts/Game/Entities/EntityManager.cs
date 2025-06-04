using Godot;
using System;
using System.Collections.Generic;
using Safari.Scripts.Game.Entities.Animals.UsableTiles;
using Safari.Scripts.Game.Tiles;
using System.Threading.Tasks;
using System.Linq;
using Safari.Scripts.Game.Road;
using Safari.Scripts.Game.Entities;
public enum Difficulty
{
	Easy,
	Medium,
	Hard
}
public partial class EntityManager : Node2D
{
	public const float clickRange = 100.0f; //Range for applying chips

	[Signal] public delegate void AnimalCountChangedEventHandler(int herbivores, int carnivores);
	[Signal] public delegate void TouristCountChangedEventHandler(int count);
	[Signal] public delegate void TouristReviewChangedEventHandler(double average);
	[Signal] public delegate void GameOverEventHandler(bool won);

	private NavigationRegion2D _navRegion;
	private MapManager _mapManager;

	

	private double counter;
	private int _dayCounter;
	public int DayCounter
	{
		get => _dayCounter;
		set
		{
			_dayCounter = value;
		   
		}
	}
	private long  AnimalId;
	private Random _random;
	private Difficulty _difficulty;
	public Difficulty Difficulty
	{
		get => _difficulty;
		set
		{
			_difficulty = value;
		   
		}
	}
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
	public List<Animal> GetCorpses => Corpses;
	private List<Vector2> Holes = new();
	public List<UsableTile> GetUsableTiles => UsableTiles;
	private Random _rng = new Random();

	private Vector2 TopLeft;
	private Vector2 BottomRight;
	private Vector2 BottomLeft;
	private Vector2 TopRight;

	private bool IsGenerationFinished = false;
	private bool _isBaking = false;
	private int _countForBaking = 0;
	private NavigationPolygon MeshWithHoles;
	private NavigationMeshSourceGeometryData2D GeoMesh = new NavigationMeshSourceGeometryData2D();

	private BuyMenuOverlay _buyMenu;
	private List<Jeep> _jeeps;
	private List<Tourist> _tourists;
	private float _reviewAverage;
	private int _reviewCount;
	

	public float ReviewAverage
	{
		get => _reviewAverage;
		set
		{
			_reviewAverage = value;
			EmitSignal(SignalName.TouristReviewChanged, _reviewAverage);
		}
	}
	public int ReviewCount
	{
		get => _reviewCount;
		set
		{
			_reviewCount = value;
			
		}
	}
	
	public int GetSetAnimalId { get; set; }



	public MapManager GetMapManager() => _mapManager;
	[Export] Texture2D chipLightTexture = new Texture2D();
	public void TestAnimals()
	{
		SpawnAnimal(CreateValidCoord(), new Stag());
	}
	public override void _Ready()
	{
		_navRegion = GetNode<NavigationRegion2D>("NavigationRegion2D");
		
		_mapManager = GetNode<MapManager>(MapManagerPath);
		_buyMenu = GetNode<BuyMenuOverlay>(BuyMenuPath);
		
		_mapManager.SellAnimal += SellAnimal;
		_mapManager.Light.DayPassed += OnDayPassed;

		_buyMenu.JeepBought += OnJeepBought;
		_buyMenu.AnimalBought += OnAnimalBought;
		_buyMenu.ChipBought += OnChipBought;
		_dayCounter = 0;
		_jeeps = [];
		_tourists = [];
		_reviewAverage = 3f;
		_reviewCount = 1;
		AnimalId = 1;
		counter = 0;
		_random = new Random();
		var newNavigationMesh = new NavigationPolygon();

		// Create a new navigation mesh and add the corners of the map as the outline
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

		}
		else
		{
			GD.PrintErr("MapManager not assigned in the Inspector!");
		}
	}
	public void NewGame()
	{
		_mapManager.GenerateMap();
		for(int i = 0; i < 10; ++i)
		{
			SpawnAnimal(CreateValidCoord(), new Stag());
			SpawnAnimal(CreateValidCoord(), new Wolf());
			SpawnAnimal(CreateValidCoord(), new Boar());
			SpawnAnimal(CreateValidCoord(), new Hyena());
		}
		(int herb, int carn) = CountAnimals();
		EmitSignal(SignalName.AnimalCountChanged, herb, carn);
		
	}
	public void TestGame()
	{
		_mapManager.GenerateMap();
		for (int i = 0; i < 1; ++i)
		{
			SpawnAnimal(CreateValidCoord(), new Stag());
			SpawnAnimal(CreateValidCoord(), new Wolf());
			SpawnAnimal(CreateValidCoord(), new Boar());
			SpawnAnimal(CreateValidCoord(), new Hyena());
		}
		(int herb, int carn) = CountAnimals();
		EmitSignal(SignalName.AnimalCountChanged, herb, carn);
	}
	private void OnDayPassed()
	{
		_dayCounter++;
		List<UsableTile> Temp = new List<UsableTile>();
		foreach (var tile in UsableTiles)
		{
			if (tile.TileType is FlowerTile || tile.TileType is TreeTile)
			{
				Temp.Add(tile);
			
			
			}
		}
		
		
		//create herbivore and carnivore lists
		List<Herbivore> herbivores = _stags.Cast<Herbivore>().Concat(_boars.Cast<Herbivore>()).ToList();
		List<Carnivore> carnivores = _wolves.Cast<Carnivore>().Concat(_hyenas.Cast<Carnivore>()).ToList();
		if (_difficulty == Difficulty.Easy)
		{
		   if(_dayCounter == 60)
			{
				GameWon();
			}else if(_dayCounter > 15 && _dayCounter < 60)
			{
				if(Temp.Count < 5 || herbivores.Count < 3 || carnivores.Count < 3)
				{
					GameLost();
				}
			}
		}
		else if (_difficulty == Difficulty.Medium)
		{
			if (_dayCounter == 90)
			{
				GameWon();
			}
			else if (_dayCounter > 10 && _dayCounter < 90)
			{
				if (Temp.Count < 5 || herbivores.Count < 3 || carnivores.Count < 3)
				{
					GameLost();
				}
			}
		}
		else if (_difficulty == Difficulty.Hard)
		{
			if (_dayCounter == 120)
			{
				GameWon();
			}
			else if (_dayCounter > 5 && _dayCounter < 120)
			{
				if (Temp.Count < 10 || herbivores.Count < 10 || carnivores.Count < 10)
				{
					GameLost();
				}
			}
		}
		herbivores.Clear();
		carnivores.Clear();
		foreach (var tile in Temp)
		{
			tile.DayPassed();

		}
		Temp.Clear();

	}
	private void GameWon()
	{
		GD.Print("You won!");
		EmitSignal(SignalName.GameOver, true);
	}
	private void GameLost()
	{
		GD.Print("Game Over");
		GameVariables.Instance.IsGameOver = true;
		GetTree().Root.ProcessMode = ProcessModeEnum.Disabled;
		EmitSignal(SignalName.GameOver, false);
	}

	private void OnAnimalBought(Animal animal)
	{
		int price = (animal as Buyable).Price;
		GD.Print("Animal bought: ", price);
		

		if (!_mapManager.IsInBounds())
		{
			GD.Print("Mouse is out of bounds");
			return;
		}
		if (!GameVariables.Instance.DecreaseMoney(price))
			return;
		
		SpawnAnimal(GetLocalMousePosition(), animal);
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

	private void OnChipBought()
	{

		Buyable chip = new TrackerChip();
		int price = chip.Price;

		Vector2 pos = GetLocalMousePosition();

		(float closest_dist, Animal current_animal) = FindClosestAnimal(pos);

		if(closest_dist > clickRange)
			return;

		if (!GameVariables.Instance.DecreaseMoney(price))
			return;

		if(current_animal.hasChip)
			return;

		PointLight2D chipLight = new PointLight2D();
		chipLight.Color = Colors.DarkRed;
		chipLight.BlendMode = Light2D.BlendModeEnum.Add;
		chipLight.Enabled = true;
		chipLight.Texture = chipLightTexture;
		chipLight.Energy = 0.8f;
		chipLight.TextureScale = 0.2f;
		chipLight.Height = 100;
		chipLight.RangeLayerMin = 0;

		current_animal.AddChild(chipLight);
		current_animal.hasChip = true;
	}

	private void SellAnimal(){
		if (GameVariables.Instance.IsGameOver)
			return;
		Vector2 pos = GetLocalMousePosition();
		(float closest_dist, Animal animal) = FindClosestAnimal(pos);
		Buyable chip = new TrackerChip();

		if(closest_dist > clickRange)
			return;

		GameVariables.Instance.AddMoney((animal as Buyable).Price);
		if(animal.hasChip)
			GameVariables.Instance.AddMoney(chip.Price);

		RemoveAnimal(animal);
		(int herb, int carn) = CountAnimals();
		EmitSignal(SignalName.AnimalCountChanged, herb, carn);
		animal.DestroySelf();
	}

	private void OnLeaveReview(float review)
	{
		float sum = _reviewAverage * _reviewCount + review;
		_reviewCount++;
		_reviewAverage = sum / _reviewCount;
		EmitSignal(SignalName.TouristReviewChanged, _reviewAverage);
	}

	private (float, Animal) FindClosestAnimal(Vector2 pos){
		List<Animal> animals = new List<Animal>();
		animals.AddRange(_wolves);
		animals.AddRange(_stags);
		animals.AddRange(_boars);
		animals.AddRange(_hyenas);

		float closest_dist = pos.DistanceTo(animals[0].Position);
		Animal current_animal = animals[0];

		for (int i = 1; i < animals.Count; ++i)
		{
			if(pos.DistanceTo(animals[i].Position) < closest_dist)
			{
				closest_dist = pos.DistanceTo(animals[i].Position);
				current_animal = animals[i];
			}
		}

		animals.Clear();
		return(closest_dist, current_animal);
	}

	private (int, int) CountAnimals()
	{
		return (_stags.Count + _boars.Count, _wolves.Count + _hyenas.Count);
	}

	
	

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
			EmitSignal(SignalName.TouristCountChanged, _tourists.Count);
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
		EmitSignal(SignalName.TouristCountChanged, _tourists.Count);
	}
	//-------------------ANIMAL FUNCTIONS-------------------
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
		animal.AnimalThirsty -= OnThirstyAnimal;
		animal.AnimalHungry -= OnAnimalHungry;
		animal.AnimalDied -= OnAnimalDied;
		animal.AnimalSee -= OnAnimalSee;
		animal.AnimalWillBeDeleted -= OnAnimalWillBeDeleted;

		EmitSignal(nameof(AnimalCountChanged), _stags.Count + _wolves.Count + _boars.Count + _hyenas.Count);
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
			foreach (Animal corpse in Corpses.ToList()) 
			{
				try
				{
					var dist = ((Carnivore)animal).GlobalPosition.DistanceTo(corpse.GlobalPosition); 
					if (dist < 1000)
					{
						if (!((Carnivore)animal).Food.Contains(corpse))
							((Carnivore)animal).Food.Add(corpse);
					}
				}
				catch (ObjectDisposedException)
				{
					
					GD.Print($"Disposed corpse (probably already removed from scene) detected. Removing from Corpses list.");
					Corpses.Remove(corpse);
				}
				catch (Exception ex)
				{
					GD.Print($"Unexpected error with corpse ID {corpse.Id}: {ex.Message}. Removing.");
					Corpses.Remove(corpse);
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
		RemoveAnimal(animal);
		(int herb, int carn) = CountAnimals();
		EmitSignal(SignalName.AnimalCountChanged, herb, carn);
		Corpses.Add(animal);
	}

	private void RemoveAnimal(Animal animal){
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
	}
	public void SpawnAnimal(Vector2 position,Animal animal){
		if (GameVariables.Instance.IsGameOver)
			return;
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
		animal.QueueFree(); 
		SpawnedAnimal.Id = AnimalId++;
		SpawnedAnimal.GlobalPosition = position;
		SpawnedAnimal.AnimalThirsty += OnThirstyAnimal;
		SpawnedAnimal.AnimalHungry += OnAnimalHungry;
		SpawnedAnimal.AnimalDied += OnAnimalDied;
		SpawnedAnimal.AnimalSee += OnAnimalSee;
		SpawnedAnimal.AnimalWillBeDeleted += OnAnimalWillBeDeleted;
		AddChild(SpawnedAnimal);

		SpawnedAnimal.NewAnimal();
		(int herb, int carn) = CountAnimals();
		EmitSignal(SignalName.AnimalCountChanged, herb, carn);
		

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

		//when the player removes the tile(or if its vegetation and it died), remove it from the animals
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
		
		usable.TileDied += OnTileDied;
		UsableTiles.Add(usable);
		
	}
	private void OnTileDied(Vector2I mapcoord,Tile tile)
	{
		RemoveTileFromAnimals(mapcoord, tile);
		OnSoledObstacle(mapcoord, tile);
		_mapManager.RemoveTile(mapcoord);
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
		if (_countForBaking == 0)
		{
			_countForBaking++;
			StartBaking();
		}
		else
		{
			_countForBaking++;
		}
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

	private async void StartBaking()
	{
		//baking navigation mesh it needs to be on a separate thread because it freezes the game

		if (!_isBaking)
		{
			_isBaking = true;
			GD.PrintErr("Baking started");
			NavigationPolygon navMesh = (NavigationPolygon)_navRegion.NavigationPolygon.Duplicate();

			_navRegion.NavigationPolygon = null;
			_navRegion.NavigationPolygon = navMesh;
			_countForBaking = 1;
			await Task.Run(() =>
			{
				NavigationServer2D.BakeFromSourceGeometryData(MeshWithHoles, GeoMesh);
				_navRegion.CallDeferred("set_navigation_polygon", MeshWithHoles);
			});

			_countForBaking--;
			if (_countForBaking > 0)
			{
				_isBaking = false;
				StartBaking();

			}
			
		}
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
		// Create a random position within the bounds of the best fitting circle of the map so that animals wont get stuck
		//they can't go out of the map 
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
			usable.Age = save.Age;
			usable.TileDied += OnTileDied;

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
			save.hasChip = herb.hasChip;
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
			newAnimal.hasChip = save.hasChip;
			if(newAnimal.hasChip)
			{
				PointLight2D chipLight = new PointLight2D();
				chipLight.Color = Colors.DarkRed;
				chipLight.BlendMode = Light2D.BlendModeEnum.Add;
				chipLight.Enabled = true;
				chipLight.Texture = chipLightTexture;
				chipLight.Energy = 0.8f;
				chipLight.TextureScale = 0.2f;
				chipLight.Height = 100;
				chipLight.RangeLayerMin = 0;
				newAnimal.AddChild(chipLight);
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
		EmitSignal(SignalName.AnimalCountChanged, _stags.Count + _wolves.Count + _boars.Count + _hyenas.Count);
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
				save.HasChip = corpse.hasChip;
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
			corpse.hasChip = save.HasChip;
			if (corpse.hasChip)
			{
				PointLight2D chipLight = new PointLight2D();
				chipLight.Color = Colors.DarkRed;
				chipLight.BlendMode = Light2D.BlendModeEnum.Add;
				chipLight.Enabled = true;
				chipLight.Texture = chipLightTexture;
				chipLight.Energy = 0.8f;
				chipLight.TextureScale = 0.2f;
				chipLight.Height = 100;
				chipLight.RangeLayerMin = 0;

				corpse.AddChild(chipLight);
			}
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
			save.hasChip = carn.hasChip;
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
			newCarnivore.hasChip = save.hasChip;
			if(newCarnivore.hasChip)
			{
				PointLight2D chipLight = new PointLight2D();
				chipLight.Color = Colors.DarkRed;
				chipLight.BlendMode = Light2D.BlendModeEnum.Add;
				chipLight.Enabled = true;
				chipLight.Texture = chipLightTexture;
				chipLight.Energy = 0.8f;
				chipLight.TextureScale = 0.2f;
				chipLight.Height = 100;
				chipLight.RangeLayerMin = 0;

				newCarnivore.AddChild(chipLight);
			}
			// Connect signals
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
		(int herb, int carn) = CountAnimals();
		EmitSignal(SignalName.AnimalCountChanged, herb, carn);
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
			jeep.TouristLoaded += OnTouristLoaded;
			jeep.Load(save, _mapManager, _mapManager.parkingLot.GetFreeSlot());
			JeepParkingSlot slot = _mapManager.parkingLot.GetFreeSlot();
			AddChild(jeep);
			_jeeps.Add(jeep);
			jeep.TouristLoaded -= OnTouristLoaded;
		}

		EmitSignal(SignalName.TouristCountChanged, _jeeps.Count);
		GD.PrintErr("                  Tourists loaded: " + _tourists.Count);
	}
	public void OnTouristLoaded(Tourist tourist)
	{
		tourist.Review += OnLeaveReview;
		EmitSignal(SignalName.TouristCountChanged, _tourists.Count);
	}
	public int SaveTourist()
	{
		return _tourists.Count;
	}
	public void LoadTourist(int count)
	{
		for (int i = 0; i < count; i++)
		{
			Tourist tourist = new Tourist();
			tourist.Review += OnLeaveReview;
			AddChild(tourist);
			_tourists.Add(tourist);
		}
		EmitSignal(SignalName.TouristCountChanged, _tourists.Count);
		
	}


	public LightSave SaveLight()
	{
		return _mapManager.SaveLight();
	}
	public void LoadLight(LightSave lightSave)
	{
		_mapManager.LoadLight(lightSave);
	}
	public override void _ExitTree()
	{
		//get all animals
		List<Animal> allAnimals = _stags.Cast<Animal>().Concat(_wolves).Concat(_boars).Concat(_hyenas).ToList();
		foreach (Animal animal in allAnimals)
		{
			animal.AnimalThirsty -= OnThirstyAnimal;
			animal.AnimalHungry -= OnAnimalHungry;
			animal.AnimalDied -= OnAnimalDied;
			animal.AnimalSee -= OnAnimalSee;
			animal.AnimalWillBeDeleted -= OnAnimalWillBeDeleted;
		}
		foreach (var jeep in _jeeps)
		{
			jeep.QueueFree();
		}
		foreach (var tourist in _tourists)
		{
			tourist.QueueFree();
		}
		allAnimals.Clear();
		base._ExitTree();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		SpawnTourist();
		StartJeep();
		
		if (counter == 0)
		{

			GroupUpAllAnimals();


		}
		if ((int)Math.Floor(counter) % 500 == 0)
			{
			
			ReproduceAllAnimal();
			
		}
		counter += delta * 10;
		
		if (counter > 2000)
			{
				counter = 0;

			
		}

	} 
}
