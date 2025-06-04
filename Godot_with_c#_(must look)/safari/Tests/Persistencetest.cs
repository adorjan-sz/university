using GdUnit4;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GdUnit4.Executions;
using static GdUnit4.Assertions;
using Safari.Scripts.Game;
using Safari.Scripts.Game.Entities;
namespace Safari.Tests
{
    [TestSuite]
    public class PersistenceTest
    {
        MainModel mainModel;
        ISceneRunner _sceneRunner;
        MapManager _mapManager;
        MapData _mapData;
        EntityManager _entityManager;
        CanvasLayer _uiLayer;
        GameSaveData saveData;
        LightManager _lightManager;
        string SavePath => $"user://{GameVariables.Instance.ParkName}.json";
        [Before]
        public async Task SetupFirst()
        {


            _sceneRunner = ISceneRunner.Load("res://Scenes/Game/MainModel.tscn");
            mainModel = (MainModel)_sceneRunner.Scene();
            await _sceneRunner.AwaitIdleFrame();
            _entityManager = AutoFree((EntityManager)_sceneRunner.FindChild("EntityManager").UnboxVariant());
            _mapManager = AutoFree((MapManager)_entityManager.FindChild("MapManager").UnboxVariant());
            _mapData = _mapManager.GetMapData();
            _lightManager = _mapManager.Light;
            _uiLayer = AutoFree((CanvasLayer)_sceneRunner.FindChild("GameOverlay").UnboxVariant());
            GameVariables.Instance.ParkName = "Test";
            
            // Simulate a new game
            mainModel.NewGame();
            GameVariables.Instance.SetMoney(9999);
            GameVariables.Instance.StartingTicketPrice = 123;
            await _sceneRunner.AwaitIdleFrame(); // Let any game logic run
            
            mainModel.SaveGame();
            //await _sceneRunner.AwaitMillis(1000);
            if (!Godot.FileAccess.FileExists(SavePath))
            {
                GD.PrintErr("No save file found!");
                return;
            }

            Godot.FileAccess file = Godot.FileAccess.Open(SavePath, Godot.FileAccess.ModeFlags.Read);
            string json = file.GetAsText();
            file.Close();

            saveData = JsonSerializer.Deserialize<GameSaveData>(json);



        }
        [TestCase]
        public void SaveDataExists()
        {
            // Check if the save file exists
            AssertThat(Godot.FileAccess.FileExists(SavePath)).IsTrue();
            AssertThat(saveData).IsNotNull();
        }
        [TestCase]
        public void GameVariablesTest()
        {
           AssertThat(saveData.ParkName).IsEqual("Test");
            AssertThat(saveData.Money).IsEqual(9999);
            AssertThat(saveData.TicketPrice).IsEqual(123);
        }
        [TestCase] 
        public void MapTest()
        {
            // Check if the map data is not null
            AssertThat(saveData.Tiles).IsNotNull();
            AssertThat(saveData.Tiles.Count).IsGreater(0);

            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 200; j++)
                {
                    var tile = saveData.Tiles.FirstOrDefault(t => t.X == i && t.Y == j);
                    AssertThat(tile).IsNotNull();
                    AssertThat(tile.X).IsEqual(i);
                    AssertThat(tile.Y).IsEqual(j);
                    
                    AssertThat(_mapData.GetTile(new Vector2I(i,j),tile.Layer).GetType().Name).IsEqual(tile.TileType);

                }
            }


        }
        [TestCase]
        public void UsableTilesTest()
        {
            AssertThat(saveData.UsableTiles).IsNotNull();
            AssertThat(saveData.UsableTiles.Count).IsGreater(0);

            foreach (var usableTile in saveData.UsableTiles)
            {
                // Basic property checks
                AssertThat(usableTile.TileTypeName).IsNotNull();
                AssertThat(usableTile.TileTypeName.Length).IsGreater(0);
                AssertThat(usableTile.MapX).IsGreaterEqual(0);
                AssertThat(usableTile.MapY).IsGreaterEqual(0);
                AssertThat(usableTile.WorldX).IsNotEqual(0); // assuming valid world coords aren't zero
                AssertThat(usableTile.WorldY).IsNotEqual(0);
                var u = _entityManager.GetUsableTiles.FirstOrDefault(t => t.MapCoords.X == usableTile.MapX && t.MapCoords.Y == usableTile.MapY);
                AssertThat(u).IsNotNull();
                AssertThat(u.Age).IsEqual(usableTile.Age);
                AssertThat(u.TileType.GetType().Name).IsEqual(usableTile.TileTypeName);

                // Optional: check that tile type exists in the factory (if desired)
                var tile = TileFactory.CreateTile(usableTile.TileTypeName);
                AssertThat(tile).IsNotNull(); // Ensures the tile type is known and constructible
            } 
        }
        [TestCase]
        public void HerbivoreSaveLoadTest()
        {
           

          

           foreach(var herbivore in saveData.Herbivores)
            {
                // create a herbivore list from stags and boars
                var herbivoreList = _entityManager.Stags.Cast<Herbivore>().Concat(_entityManager.Boars).ToList();
                var CorrectHerbivore = herbivoreList.FirstOrDefault(h => h.Id ==herbivore.Id);
                AssertThat(CorrectHerbivore).IsNotNull();
                AssertThat(CorrectHerbivore.Id).IsEqual(herbivore.Id);
                AssertThat(Math.Abs(CorrectHerbivore.StateMachine.GetAge() - herbivore.Age)).IsLess(1);
                AssertThat(Math.Abs(CorrectHerbivore.StateMachine.GetHunger() - herbivore.Hunger)).IsLess(1);
                AssertThat(Math.Abs(CorrectHerbivore.StateMachine.GetThirst() - herbivore.Thirst)).IsLess(1);
                AssertThat(Math.Abs(CorrectHerbivore.GlobalPosition.X - herbivore.PosX)).IsLess(1);
                AssertThat(Math.Abs(CorrectHerbivore.GlobalPosition.Y - herbivore.PosY)).IsLess(1);
                AssertThat(CorrectHerbivore.StateMachine.CurrentState).IsEqual(CorrectHerbivore.StateMachine.States[(herbivore.CurrentStateName)]);


                // Assert water tiles
                var waterCoords = CorrectHerbivore.Water.Select(w => w.MapCoords).ToList();
                for (int i = 0; i < herbivore.WaterTileXs.Count; i++)
                {
                    var expectedCoord = new Vector2I(herbivore.WaterTileXs[i], herbivore.WaterTileYs[i]);
                    AssertThat(waterCoords).Contains(expectedCoord);
                }

                // Assert food tiles
                var foodCoords = CorrectHerbivore.Food.Select(f => f.MapCoords).ToList();
                for (int i = 0; i < herbivore.FoodTileXs.Count; i++)
                {
                    var expectedCoord = new Vector2I(herbivore.FoodTileXs[i], herbivore.FoodTileYs[i]);
                    AssertThat(foodCoords).Contains(expectedCoord);
                }

                // Assert current water target
                if (herbivore.CurrentWaterX.HasValue && herbivore.CurrentWaterY.HasValue)
                {
                    var expected = new Vector2I(herbivore.CurrentWaterX.Value, herbivore.CurrentWaterY.Value);
                    AssertThat(CorrectHerbivore.CurrentWaterToGo).IsNotNull();
                    AssertThat(CorrectHerbivore.CurrentWaterToGo.MapCoords).IsEqual(expected);
                }
                else
                {
                    AssertThat(CorrectHerbivore.CurrentWaterToGo).IsNull();
                }

                // Assert current vegetation target
                if (herbivore.CurrentVegetationX.HasValue && herbivore.CurrentVegetationY.HasValue)
                {
                    var expected = new Vector2I(herbivore.CurrentVegetationX.Value, herbivore.CurrentVegetationY.Value);
                    AssertThat(CorrectHerbivore.CurrentVegetationToEat).IsNotNull();
                    AssertThat(CorrectHerbivore.CurrentVegetationToEat.MapCoords).IsEqual(expected);
                }
                else
                {
                    AssertThat(CorrectHerbivore.CurrentVegetationToEat).IsNull();
                }

                // Assert group-up coordinate
                if (herbivore.CurrentGroupUpX.HasValue && herbivore.CurrentGroupUpY.HasValue)
                {
                    var expected = new Vector2(herbivore.CurrentGroupUpX.Value, herbivore.CurrentGroupUpY.Value);
                    AssertThat(CorrectHerbivore.GroupUpCoord.HasValue).IsTrue();
                    AssertThat(CorrectHerbivore.GroupUpCoord.Value).IsEqual(expected);
                }
                else
                {
                    AssertThat(CorrectHerbivore.GroupUpCoord.HasValue).IsFalse();
                }

                // Assert chip status
                AssertThat(CorrectHerbivore.hasChip).IsEqual(herbivore.hasChip);
            
           }

        }
        [TestCase]
        public void TestCarnivore()
        {
            foreach (var carnivore in saveData.Carnivores)
            {
                var carnivoreList = _entityManager.Wolves.Cast<Carnivore>().Concat(_entityManager.Hyenas).ToList();
                var correctCarnivore = carnivoreList.FirstOrDefault(c => c.Id == carnivore.Id);
                AssertThat(correctCarnivore).IsNotNull();
                AssertThat(correctCarnivore.Id).IsEqual(carnivore.Id);
                AssertThat(Math.Abs(correctCarnivore.StateMachine.GetAge() - carnivore.Age)).IsLess(1);
                AssertThat(Math.Abs(correctCarnivore.StateMachine.GetHunger() - carnivore.Hunger)).IsLess(1);
                AssertThat(Math.Abs(correctCarnivore.StateMachine.GetThirst() - carnivore.Thirst)).IsLess(1);
                AssertThat(Math.Abs(correctCarnivore.GlobalPosition.X - carnivore.PosX)).IsLess(1);
                AssertThat(Math.Abs(correctCarnivore.GlobalPosition.Y - carnivore.PosY)).IsLess(1);
                AssertThat(correctCarnivore.StateMachine.CurrentState).IsEqual(correctCarnivore.StateMachine.States[carnivore.CurrentStateName]);

                // Assert water tiles
                var waterCoords = correctCarnivore.Water.Select(w => w.MapCoords).ToList();
                for (int i = 0; i < carnivore.WaterTileXs.Count; i++)
                {
                    var expectedCoord = new Vector2I(carnivore.WaterTileXs[i], carnivore.WaterTileYs[i]);
                    AssertThat(waterCoords).Contains(expectedCoord);
                }

                // Assert food (by Id)
                var foodIds = correctCarnivore.Food.Select(f => f.Id).ToList();
                foreach (var id in carnivore.FoodIds)
                {
                    AssertThat(foodIds).Contains(id);
                }

                // Assert current target
                if (carnivore.CurrentTargetId.HasValue)
                {
                    AssertThat(correctCarnivore.CurrentThingToEat).IsNotNull();
                    AssertThat(correctCarnivore.CurrentThingToEat.Id).IsEqual(carnivore.CurrentTargetId.Value);
                }
                else
                {
                    AssertThat(correctCarnivore.CurrentThingToEat).IsNull();
                }

                // Assert group-up coordinate
                if (carnivore.CurrentGroupUpX.HasValue && carnivore.CurrentGroupUpY.HasValue)
                {
                    var expected = new Vector2(carnivore.CurrentGroupUpX.Value, carnivore.CurrentGroupUpY.Value);
                    AssertThat(correctCarnivore.GroupUpCoord.HasValue).IsTrue();
                    AssertThat(correctCarnivore.GroupUpCoord.Value).IsEqual(expected);
                }
                else
                {
                    AssertThat(correctCarnivore.GroupUpCoord.HasValue).IsFalse();
                }

                // Assert water target
                if (carnivore.CurrentWaterX.HasValue && carnivore.CurrentWaterY.HasValue)
                {
                    var expected = new Vector2I(carnivore.CurrentWaterX.Value, carnivore.CurrentWaterY.Value);
                    AssertThat(correctCarnivore.CurrentWaterToGo).IsNotNull();
                    AssertThat(correctCarnivore.CurrentWaterToGo.MapCoords).IsEqual(expected);
                }
                else
                {
                    AssertThat(correctCarnivore.CurrentWaterToGo).IsNull();
                }

                // Assert chip status
                AssertThat(correctCarnivore.hasChip).IsEqual(carnivore.hasChip);
            }
        }
        [TestCase]
        public void TestCorpseLoading()
        {
            foreach (var corpse in saveData.Corpse)
            {
                var foundCorpse = _entityManager.GetCorpses.FirstOrDefault(c => c.Id == corpse.Id);
               

                AssertThat(foundCorpse).IsNotNull();
                AssertThat(foundCorpse.Id).IsEqual(corpse.Id);
                AssertThat(foundCorpse.GetType().Name).IsEqual(corpse.AnimalType);
                AssertThat(foundCorpse.GlobalPosition.X).IsEqual(corpse.PosX);
                AssertThat(foundCorpse.GlobalPosition.Y).IsEqual(corpse.PosY);
                AssertThat(foundCorpse.hasChip).IsEqual(corpse.HasChip);

                var deathState = foundCorpse.StateMachine.CurrentState as DeathState;
                AssertThat(deathState).IsNotNull();
                AssertThat(deathState.GetCount()).IsEqual(corpse.DeathCount);
            }
        }
        [TestCase]
        public void TestLightManagerLoad()
        {
            LightSave lightSave = saveData.Lights;
            AssertThat(lightSave).IsNotNull();
            AssertThat(lightSave.LightPositionsX).IsNotNull();
            AssertThat(lightSave.LightPositionsY).IsNotNull();
            AssertThat(lightSave.LightPositionsX.Count).IsEqual(lightSave.LightPositionsY.Count);
            List<PointLight2D> lights = _lightManager.GetLights;
            for(int i = 0; i < lightSave.LightPositionsX.Count; i++)
            {
                AssertThat(lights[i].Offset.X).IsEqual(lightSave.LightPositionsX[i]);
                AssertThat(lights[i].Offset.Y).IsEqual(lightSave.LightPositionsY[i]);
            }
            AssertThat(Math.Abs(_lightManager.GetCurrentTime- lightSave.CurrentTime)).IsLess(10);
            AssertThat(_lightManager.GetDayLightState.ToString()).IsEqual(lightSave.CurrentDayState);
            AssertThat(_lightManager.GetLightsON).IsEqual(lightSave.LightsOn);





        }
        [TestCase]
        public void Jeep_and_TouristTest()
        {
            foreach (var jeepSave in saveData.Jeeps)
            {
                var jeepList = _entityManager.GetChildren().OfType<Jeep>().ToList();
                var correctJeep = jeepList.FirstOrDefault(j =>
                    Math.Abs(j.Position.X - jeepSave.PosX) < 0.1 &&
                    Math.Abs(j.Position.Y - jeepSave.PosY) < 0.1 &&
                    j.StateString == jeepSave.State);

                AssertThat(correctJeep).IsNotNull();
                AssertThat(correctJeep.Position.X).IsEqual(jeepSave.PosX);
                AssertThat(correctJeep.Position.Y).IsEqual(jeepSave.PosY);
                AssertThat(correctJeep.StateString).IsEqual(jeepSave.State);
                AssertThat(correctJeep.NextCell.X).IsEqual(jeepSave.NextCellX);
                AssertThat(correctJeep.NextCell.Y).IsEqual(jeepSave.NextCellY);

                AssertThat(correctJeep.PassengerCount).IsEqual(jeepSave.Passengers.Count);

                for (int i = 0; i < jeepSave.Passengers.Count; i++)
                {
                    TouristSave expected = jeepSave.Passengers[i];
                    Tourist actual = correctJeep.Passengers[i];
                    AssertThat(actual).IsNotNull();
                    AssertThat(actual.TotalAnimalSeen).IsEqual(expected.TotalAnimalSeen);
                    AssertThat(actual.SpeciesCounts.Count).IsEqual(expected.SpeciesCounts.Count);

                }
            }
        }
        [TestCase]
        public void MiscellaneousGameDataTest()
        {
            AssertThat(saveData.ReviewAverage).IsLessEqual(_entityManager.ReviewAverage);
            AssertThat(saveData.ReviewCount).IsLessEqual(_entityManager.ReviewCount);
            AssertThat(saveData.AnimalId).IsEqual(_entityManager.GetSetAnimalId);
            AssertThat(saveData.GameDifficulty).IsEqual(_entityManager.Difficulty.ToString());
            AssertThat(saveData.TouristCount).IsEqual(_entityManager.GetChildren().OfType<Tourist>().Count());
            AssertThat(saveData.Day).IsEqual(_entityManager.DayCounter);
        }











    }
}

