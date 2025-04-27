using ModelAndPersistencia.Model;
using ModelAndPersistencia.Persistence;
namespace play2048
{
    public partial class Form1 : Form
    {
        private Button[,] _buttonGrid = null!;
        private GameModel _gameModel;
        public Form1()
        {
            InitializeComponent();
            IDataAccess dataAccess = new DataAccess();
            _gameModel = new GameModel(dataAccess);
            this.KeyDown += new KeyEventHandler(KeyCheck);
            this.KeyPreview = true;
            GenerateTable();
            _gameModel.TableChanged += new EventHandler<TableEventArgs>(ButtonUpdate);
            _gameModel.GameOver += new EventHandler(GameOver);

            _gameModel.NewGame();

        }

        private void GenerateTable()
        {
            _buttonGrid = new Button[_gameModel.TableSize, _gameModel.TableSize];
            for (Int32 i = 0; i < _gameModel.TableSize; i++)
                for (Int32 j = 0; j < _gameModel.TableSize; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(5 + 50 * j, 35 + 50 * i); // elhelyezkedés
                    _buttonGrid[i, j].Size = new Size(50, 50); // méret
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                    _buttonGrid[i, j].Enabled = false; // kikapcsolt állapot
                    _buttonGrid[i, j].TabIndex = 100 + i * _gameModel.TableSize + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    
                    //_buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    // közös eseménykezelõ hozzárendelése minden gombhoz

                    Controls.Add(_buttonGrid[i, j]);
                    // felvesszük az ablakra a gombot
                }
        }

        /// <summary>
        /// Tábla beállítása.
        /// </summary>
        private void SetupTable()
        {
            for (Int32 i = 0; i < _buttonGrid.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _buttonGrid.GetLength(1); j++)
                {
                    
                    _buttonGrid[i, j].Text = (_gameModel.getValue(i, j)==0)
                            ? String.Empty
                            : _gameModel.getValue(i, j).ToString();
                    
                }
            }

            //_toolLabelGameSteps.Text = _model.GameStepCount.ToString();
            //_toolLabelGameTime.Text = TimeSpan.FromSeconds(_model.GameTime).ToString("g");
        }
        private void ButtonUpdate(Object? sender, TableEventArgs e)
        {
            SetupTable();
        }
        private void NewGameButton_Clicked(Object sender, EventArgs e)
        {
            _gameModel.NewGame();
        }
        private void GameOver(Object? sender, EventArgs e)
        {
            MessageBox.Show("Gratulálok, gyõztél!");
        }
        private void KeyCheck(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case (Keys.W):
                    _gameModel.MoveUp(); break;
                case (Keys.D):
                    _gameModel.MoveRight(); break;
                case (Keys.S):
                   _gameModel.MoveDown(); break;
                case (Keys.A):
                    _gameModel.MoveLeft(); break;


            }
        }
    }
}
