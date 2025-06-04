using GdUnit4;
using static GdUnit4.Assertions;
using Godot;
using System;
using System.Threading.Tasks;

[TestSuite]
public partial class LightTest
{
    ISceneRunner _sceneRunner;
    EntityManager _entityManager;
    LightManager _lightManager;
    MapManager _mapManager;

    [Before]
    public async Task SetupFirst(){
        _sceneRunner = ISceneRunner.Load("res://Scenes/Game/MainModel.tscn");
        await _sceneRunner.AwaitIdleFrame();
        _entityManager = AutoFree((EntityManager)_sceneRunner.FindChild("EntityManager").UnboxVariant());
        _entityManager.TestGame();

        _mapManager = AutoFree((MapManager)_entityManager.FindChild("MapManager").UnboxVariant());
		
        _lightManager = AutoFree((LightManager)_mapManager.FindChild("MainLight"));
    }

    [TestCase]
    public void SetUp()
    {
    	AssertThat(_sceneRunner).IsNotNull();
    	AssertThat(_entityManager).IsNotNull();
    	AssertThat(_mapManager).IsNotNull();
        AssertThat(_lightManager).IsNotNull();
    }

    [TestCase]
    public void CheckStart(){
        AssertThat(_lightManager.GetCurrentTime).IsLess(8 *60+5);
        AssertThat(_lightManager.GetDayLightState).IsEqual(DayState.DAY);
        AssertThat(_lightManager.GetLightsON).IsFalse();
        AssertThat(_lightManager.GetLightPos().Count).IsEqual(0);
    }

    [TestCase]
    public async Task TimeChangedSignalTest(){
        bool timeChanged = false;
        _lightManager.TimeChanged += OnTimeChanged;


        void OnTimeChanged(double delta){
            timeChanged = true;
        }

        await _lightManager.ToSignal(_lightManager.GetTree().CreateTimer(1), "timeout");
        AssertThat(timeChanged).IsTrue();

    }

    [TestCase]
    public async Task NightTest(){
        _lightManager.GetCurrentTime = 24*8;

        await _lightManager.ToSignal(_lightManager.GetTree().CreateTimer(0.5f), "timeout");

        AssertThat(_lightManager.GetDayLightState).IsEqual(DayState.NIGHT);
        AssertThat(_lightManager.GetLightsON).IsTrue();
    }

    [TestCase]
    public void LightPlacementTest(){
        _mapManager.EmitSignal(MapManager.SignalName.LightPlaced, new Vector2(1, 1));

        AssertThat(_lightManager.GetLightPos().Count).IsEqual(1);
        AssertThat(_lightManager.GetLightPos()[0]).IsEqual(new Vector2(1, 1));
    }

    [TestCase]
    public void LightSoldTest(){
        _mapManager.EmitSignal(MapManager.SignalName.LightSold, new Vector2(1, 1));

        AssertThat(_lightManager.GetLightPos().Count).IsEqual(0);
    }
    
}
