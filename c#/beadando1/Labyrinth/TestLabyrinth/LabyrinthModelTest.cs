using Labyrinth.Model;
using Labyrinth.Persistence;
using Moq;
using System.Security.Cryptography.X509Certificates;
namespace TestLabyrinth
{
    [TestClass]
    public class LabyrinthModelTest
    {
        [TestClass]
        public class LabyrinthGameModelTest
        {
            private LabyrinthGameModel _model = null!;
            private LabyrinthTable _mockedTableEasy = null!;
            private LabyrinthTable _mockedTableMedium = null!;
            private LabyrinthTable _mockedTableHard = null!;
            private Mock<ILabyrinthDataAccess> _mock = null!;
            [TestInitialize]
            public void Initialize()
            {
                _mockedTableEasy = new LabyrinthTable(10);
                _mockedTableMedium = new LabyrinthTable(12);
                _mockedTableHard = new LabyrinthTable(12);
                _mock = new Mock<ILabyrinthDataAccess>();
                _mock.Setup(mock => mock.LoadEasy()).Returns(_mockedTableEasy);
                _mock.Setup(mock => mock.LoadMedium()).Returns(_mockedTableMedium);
                _mock.Setup(mock => mock.LoadHard()).Returns(_mockedTableHard);
                _model = new LabyrinthGameModel(_mock.Object);
                _model.GameAdvanced += new EventHandler<LabyrinthEventArgs>(Model_GameAdvanced);
                _model.GameOver += new EventHandler<LabyrinthEventArgs>(Model_GameOver);
            }
            [TestMethod]
            public void LabyrinthGameModelNewGameMediumTest()
            {
                _model.GameDifficulty = GameDifficulty.Medium;
                _model.NewGame();

                Assert.AreEqual(0, _model.GameTime); 
                Assert.AreEqual(GameDifficulty.Medium, _model.GameDifficulty);
                Assert.AreEqual(_model.Table.Size-1, _model.player.X);
                Assert.AreEqual(0, _model.player.Y);
                Assert.AreEqual(12, _model.Table.Size);


            }
            [TestMethod]
            public void LabyrinthGameModelNewGameEasyTest()
            {
                _model.GameDifficulty = GameDifficulty.Easy;
                _model.NewGame();

                
                Assert.AreEqual(GameDifficulty.Easy, _model.GameDifficulty); 
                Assert.AreEqual(0, _model.GameTime); 
                Assert.AreEqual(_model.Table.Size-1, _model.player.X);
                Assert.AreEqual(0, _model.player.Y);
                Assert.AreEqual(10, _model.Table.Size);//10nek k�ne lennie
            }
            [TestMethod]
            public void LabyrinthGameModelNewGameHardTest()
            {
                _model.GameDifficulty = GameDifficulty.Hard;
                _model.NewGame();


                Assert.AreEqual(GameDifficulty.Hard, _model.GameDifficulty);
                Assert.AreEqual(0, _model.GameTime);
                Assert.AreEqual(_model.Table.Size - 1, _model.player.X);
                Assert.AreEqual(0, _model.player.Y);
                Assert.AreEqual(12, _model.Table.Size);
            }
            [TestMethod]
            public void LabyrinthGameModelStepTest()
            {
                

                _model.NewGame();
                Assert.AreEqual(0, _model.player.Y);
                Assert.AreEqual(_model.Table.Size - 1, _model.player.X);
                Assert.IsFalse(_model.IsOver);
                Random random = new Random();
                

                do
                {

                    _model.Step(Direction.Right);
                } while (_model.player.Y != _model.Table.Size - 1);
                do
                {

                    _model.Step(Direction.Up);
                } while (_model.player.X != 0);

                //Addig megy amig a j�t�knak nics v�ge                
                Assert.IsTrue(_model.IsOver);
                //nem lehet l�pni ha v�ge
                _model.Step(Direction.Down);
                Assert.AreEqual(0, _model.player.X);
                Assert.AreEqual(_model.Table.Size - 1, _model.player.Y);
            }
            [TestMethod]
            public void LabyrinthGameModelAdvanceTimeTest()
            {
                _model.NewGame();

                Int32 time = _model.GameTime;
                
                _model.AdvanceTime();
                time++;
                Assert.AreEqual(time, _model.GameTime);

                //menj�nk a c�lba...
                do
                {

                    _model.Step(Direction.Right);
                } while (_model.player.Y != _model.Table.Size - 1);
                do
                {

                    _model.Step(Direction.Up);
                } while (_model.player.X != 0);

                //v�ge ut�n nem telhet az id�
                time= _model.GameTime;
                //id� meg�llt m�r
                _model.AdvanceTime();
                Assert.AreEqual(time,_model.GameTime);
            }
            private void Model_GameAdvanced(Object? sender, LabyrinthEventArgs e)
            {
                Assert.IsTrue(_model.GameTime >= 0); // a j�t�kid� nem lehet negat�v
                Assert.AreEqual(_model.player.Y == _model.Table.Size - 1 && _model.player.X == 0, _model.IsOver); 

                
                Assert.AreEqual(e.GameTime, _model.GameTime); // a k�t �rt�knek egyeznie kell
                Assert.IsFalse(e.IsWon); // m�g nem nyert�k meg a j�t�kot
            }

            private void Model_GameOver(Object? sender, LabyrinthEventArgs e)
            {
                Assert.IsTrue(_model.IsOver); // biztosan v�ge van a j�t�knak
                Assert.AreEqual(_model.player.Y == _model.Table.Size - 1 && _model.player.X == 0, _model.IsOver);
                Assert.IsTrue(e.IsWon);
            }

        }
    }
}