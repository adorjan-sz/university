using System;
using System.IO;
using System.Threading.Tasks;
using ELTE.Sudoku.Model;
using ELTE.Sudoku.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ELTE.Sudoku.Test
{
    public class MockTimer : ITimer
    {
        public bool Enabled { get; set; }
        public double Interval { get; set; }
        public event EventHandler? Elapsed;

        public void Start()
        {
            Enabled = true;
        }

        public void Stop()
        {
            Enabled = false;
        }

        /// <summary>
        /// Időzítő eseményének explicit kiváltása.
        /// </summary>
        public void RaiseElapsed()
        {
            Elapsed?.Invoke(this, EventArgs.Empty);
        }
    }
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
            private MockTimer _mockedTimer = new MockTimer(); // az időzítő mock-ja
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
                _model = new LabyrinthGameModel(_mock.Object, _mockedTimer);
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
                Assert.AreEqual(_model.Table.Size - 1, _model.player.X);
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
                Assert.AreEqual(_model.Table.Size - 1, _model.player.X);
                Assert.AreEqual(0, _model.player.Y);
                Assert.AreEqual(10, _model.Table.Size);//10nek kéne lennie
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

                //Addig megy amig a játéknak nics vége                
                Assert.IsTrue(_model.IsOver);
                //nem lehet lépni ha vége
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

                //menjünk a célba...
                do
                {

                    _model.Step(Direction.Right);
                } while (_model.player.Y != _model.Table.Size - 1);
                do
                {

                    _model.Step(Direction.Up);
                } while (_model.player.X != 0);

                //vége után nem telhet az idő
                time = _model.GameTime;
                //idö megállt már
                _model.AdvanceTime();
                Assert.AreEqual(time, _model.GameTime);
            }
            private void Model_GameAdvanced(Object? sender, LabyrinthEventArgs e)
            {
                Assert.IsTrue(_model.GameTime >= 0); // a játékidő nem lehet negatív
                Assert.AreEqual(_model.player.Y == _model.Table.Size - 1 && _model.player.X == 0, _model.IsOver);


                Assert.AreEqual(e.GameTime, _model.GameTime); // a két értéknek egyeznie kell
                Assert.IsFalse(e.IsWon); // még nem nyerték meg a játékot
            }

            private void Model_GameOver(Object? sender, LabyrinthEventArgs e)
            {
                Assert.IsTrue(_model.IsOver); // biztosan vége van a játéknak
                Assert.AreEqual(_model.player.Y == _model.Table.Size - 1 && _model.player.X == 0, _model.IsOver);
                Assert.IsTrue(e.IsWon);
            }

        }
    }
}
