using GdUnit4;
using GdUnit4.Executions;
using static GdUnit4.Assertions;
using Godot;
using Safari.Scripts.Game.Entities.Animals;
using System.Text;
using System.Threading.Tasks;
using System;
using Safari.Scripts.Game.Entities.Animals.UsableTiles;
using System.Linq;


namespace Safari.Tests;

[TestSuite]
public class AnimalTest
{
	
	
	
	ISceneRunner _sceneRunner;
	MapManager _mapManager;
	EntityManager _entityManager;
	Camera2D _camera;
	CanvasLayer _uiLayer;
	Stag stag;
	Boar boar;
	Hyena hyena;
	Wolf wolf;
	
	[Before]
	public async Task SetupFirst()
	{
		
		
		_sceneRunner = ISceneRunner.Load("res://Scenes/Game/MainModel.tscn");
		await _sceneRunner.AwaitIdleFrame();
		_entityManager = AutoFree((EntityManager)_sceneRunner.FindChild("EntityManager").UnboxVariant());
		_entityManager.TestGame();
		await _sceneRunner.AwaitMillis(1000);
        _mapManager = AutoFree((MapManager)_entityManager.FindChild("MapManager").UnboxVariant());
		_camera = AutoFree((Camera2D)_sceneRunner.FindChild("Camera2D").UnboxVariant());
		_uiLayer = AutoFree((CanvasLayer)_sceneRunner.FindChild("GameOverlay").UnboxVariant());
		
	
		

		stag = (Stag)_entityManager.GetChild(2).UnboxVariant();
		wolf = AutoFree((Wolf)_entityManager.GetChild(3).UnboxVariant());
		boar = AutoFree((Boar)_entityManager.GetChild(4).UnboxVariant());
		hyena = AutoFree((Hyena)_entityManager.GetChild(5).UnboxVariant());
		await _sceneRunner.AwaitIdleFrame();


		




	}
	
	public void ResetAnimalState()
	{
		// Reset the state of the animals to their initial state
		stag.StateMachine.SetAge(0);
		stag.StateMachine.SetHunger(0);
		stag.StateMachine.SetThirst(0);
		stag.StateMachine.ChangeState("IdleState");
		
	}
	[TestCase]
	public void SetUp()
	{
		AssertThat(_sceneRunner).IsNotNull();
		AssertThat(_entityManager).IsNotNull();
		AssertThat(_mapManager).IsNotNull();
		AssertThat(_camera).IsNotNull();
		AssertThat(_uiLayer).IsNotNull();
		AssertThat(_entityManager.GetChildCount()).IsEqual(6);


		AssertThat(stag).IsNotNull();
		AssertThat(boar).IsNotNull();
		AssertThat(hyena).IsNotNull();
		AssertThat(wolf).IsNotNull();
		

	}

	[TestCase]
	public void Animal_HasCorrectname()
	{

		AssertThat(stag.AnimalsName).IsEqual("Stag");
		AssertThat(boar.AnimalsName).IsEqual("Boar");
		AssertThat(hyena.AnimalsName).IsEqual("Hyena");
		AssertThat(wolf.AnimalsName).IsEqual("Wolf");
	}
	
	[TestCase]
	public void Animal_IsCollisonActive()
	{

		
		AssertThat(stag.CollisonShapeEnabled).IsTrue();
		AssertThat(boar.CollisonShapeEnabled).IsTrue();
		AssertThat(hyena.CollisonShapeEnabled).IsTrue();
		AssertThat(wolf.CollisonShapeEnabled).IsTrue();
	}
	[TestCase]
	public void Animal_HasNoChipByDefault()
	{
		
		AssertThat(stag.hasChip).IsFalse();
		AssertThat(boar.hasChip).IsFalse();
		AssertThat(hyena.hasChip).IsFalse();
		AssertThat(wolf.hasChip).IsFalse();
	}
	
	[TestCase]
	public void Animal_AgeIsZeroByDefault()
	{
		AssertThat(stag.StateMachine.GetAge()).IsLess(1);
		AssertThat(boar.StateMachine.GetAge()).IsLess(1);
		AssertThat(hyena.StateMachine.GetAge()).IsLess(1);
		AssertThat(wolf.StateMachine.GetAge()).IsLess(1);
	}

	[TestCase]
	public void Animal_InitialStateIsIdle()
	{
		// Check if the initial state of the animal is Idle
		AssertThat(stag.StateMachine.CurrentState).IsInstanceOf<IdleState>();
		AssertThat(boar.StateMachine.CurrentState).IsInstanceOf<IdleState>();
		AssertThat(hyena.StateMachine.CurrentState).IsInstanceOf<IdleState>();
		AssertThat(wolf.StateMachine.CurrentState).IsInstanceOf<IdleState>();
	}

	

	[TestCase]
	public void Animal_MoveTo_ChangesGroupUpCoord()
	{
		// Move the animal to a new position and check if the GroupUpCoord is set correctly
		Vector2 targetPosition = new Vector2(5, 5);
		stag.MoveTo(targetPosition);
		AssertThat(stag.GroupUpCoord).IsEqual(targetPosition);
		AssertThat(stag._navAgent.TargetPosition).IsEqual(targetPosition);
	}

	

	

	[TestCase]
	public void Animal_ReceivesThirstySignal_WhenWaterIsLow()
	{
		// Subscribe to the AnimalThirstyEvent and simulate a thirsty condition
		bool thirstSignalReceived = false;
		stag.AnimalThirsty += OnAnimalThirsty;

		void OnAnimalThirsty(Animal animal)
		{
			thirstSignalReceived = true;
		}

		// Simulate the animal becoming thirsty
		stag.StateMachine.ChangeState("ThirstState");  // Assume that this method triggers the thirst signal
		AssertThat(thirstSignalReceived).IsTrue();
	}

	[TestCase]
	public void Animal_ReceivesHungrySignal_WhenHungerIsDetected()
	{
		// Subscribe to the AnimalHungryEvent and simulate hunger
		bool hungerSignalReceived = false;
		stag.AnimalHungry += OnAnimalHungry;

		void OnAnimalHungry(Animal animal)
		{
			hungerSignalReceived = true;
		}

		// Simulate hunger detection
		stag.StateMachine.ChangeState("HungerState");  
													   // Simulate the animal becoming hungry
		AssertThat(hungerSignalReceived).IsTrue();
	}

	[TestCase]
	public void Animal_HasSpeed()
	{
		// Check that each animal has a defined speed
		AssertThat(stag.Speed).IsGreater(0);
		AssertThat(boar.Speed).IsGreater(0);
		AssertThat(hyena.Speed).IsGreater(0);
		AssertThat(wolf.Speed).IsGreater(0);
	}

	[TestCase]
	public void Animal_MoveToNewPosition_UpdatesVelocity()
	{
		// Ensure that when the animal moves to a new position, the velocity gets updated
		Vector2 targetPosition = new Vector2(5, 5);
		stag.MoveTo(targetPosition);
		AssertThat(stag._navAgent.Velocity).IsNotEqual(Vector2.Zero);
	}
	
	[TestCase]
	public void Animal_CanChangeState()
	{
		// Test if the animal can change state in its state machine
		stag.StateMachine.ChangeState("IdleState");
		AssertThat(stag.StateMachine.CurrentState).IsInstanceOf<IdleState>();
		stag.StateMachine.ChangeState("HungerState");
		AssertThat(stag.StateMachine.CurrentState).IsInstanceOf<HungerState>();
	}
   

	[TestCase]
	public async Task Animal_DiesWhenHungerIsBig()
	{
		// Check if the animal dies when its health reaches zero
		stag.StateMachine.SetHunger(91);
		await _sceneRunner.AwaitMillis(1000);
		AssertThat(stag.StateMachine.CurrentState).IsInstanceOf<DeathState>();
		ResetAnimalState();
	}
	[TestCase]
	public async Task Animal_DiesWhenThirstIsBig()
	{
		// Check if the animal dies when its health reaches zero
		stag.StateMachine.SetThirst(91);
		await _sceneRunner.AwaitMillis(1000);
		AssertThat(stag.StateMachine.CurrentState).IsInstanceOf<DeathState>();
		ResetAnimalState();
	}
	[TestCase]
	public async Task Animal_DiesWhenAgeIsBig()
	{
		// Check if the animal dies when its health reaches zero
		stag.StateMachine.SetAge(1001);
		await _sceneRunner.AwaitMillis(1000);
		AssertThat(stag.StateMachine.CurrentState).IsInstanceOf<DeathState>();
		ResetAnimalState();
	}

	

	[TestCase]
	public void Animal_WalkAnimationPlaysWhenMoving()
	{
		// Check if the walking animation plays when the animal moves
		Vector2 moveDirection = new Vector2(1, 0);  // Move right
		stag.PlayWalkAnimation(moveDirection);
		AssertThat(stag._animatedSprite.IsPlaying()).IsTrue();
	}
	[TestCase]
	public async Task Animal_DestroysItself_WhenCalledDestroySelf_and_recieves_signal()
	{
		// Destroy the animal and check if it gets queued for removal
		bool signalEmitted = false;
		stag.AnimalWillBeDeleted += OnAnimalWillBeDeleted;

		void OnAnimalWillBeDeleted(Animal animal)
		{
			signalEmitted = true;
		}


		var initialNodeCount = _entityManager.GetChildCount();
		stag.DestroySelf();
		AssertThat(stag.IsQueuedForDeletion()).IsTrue();
		await _sceneRunner.AwaitMillis(1000);
		AssertThat(_entityManager.GetChildCount()).IsEqual(initialNodeCount - 1);
		AssertThat(signalEmitted).IsTrue();
	}
    [TestCase]
    public async Task SpawnAnimal_CreatesAndAddsAnimalCorrectly()
    {
        int initialCount = _entityManager.GetChildCount();
        int initialStagCount = _entityManager.Stags.Count;

        Vector2I spawnPosition = new Vector2I(100, 100);
        Animal dummyAnimal = new Stag(); // This gets replaced inside SpawnAnimal
        _entityManager.SpawnAnimal(spawnPosition, dummyAnimal);

        await _sceneRunner.AwaitIdleFrame();

        // Check that one new child was added to the scene
        AssertThat(_entityManager.GetChildCount()).IsEqual(initialCount + 1);

        // Check that one new Stag was added
        AssertThat(_entityManager.Stags.Count).IsEqual(initialStagCount + 1);

        // Get the newly spawned Stag
        Stag newStag = _entityManager.Stags.Last();

        // Check that position, name and Id are valid
		//
        AssertThat((newStag.GlobalPosition - spawnPosition).Length()).IsLess(10);
        AssertThat(newStag.AnimalsName).IsEqual("Stag");
        AssertThat(newStag.Id).IsGreater(0);

        // Check that it's in the scene tree
        AssertThat(newStag.GetParent()).IsEqual(_entityManager);

        // Check that signals are connected (basic check: emit manually)
        bool thirstyCalled = false;
        newStag.AnimalThirsty += (animal) => thirstyCalled = true;
        newStag.EmitSignal(nameof(Animal.AnimalThirsty), newStag);
        AssertThat(thirstyCalled).IsTrue();
    }


}
