using Labyrinth.Model;
using Labyrinth.Persistence;
namespace LabyrinthView
{
    public partial class LabyrinthWinForms : Form
    {
        #region Fields

        private LabyrinthGameModel _model = null!; // játékmodell
        private Label[,] _LabelGrid = null!;
        private System.Windows.Forms.Timer _timer = null!; // idõzítõ

        #endregion
        public LabyrinthWinForms()
        {
            InitializeComponent();
            // adatelérés példányosítása
            ILabyrinthDataAccess _dataAccess = new LabyrinthFileDataAccess();

            // modell létrehozása és az eseménykezelõk társítása
            _model = new LabyrinthGameModel(_dataAccess);

            _model.PlayerMoved += new EventHandler<LabyrinthPlayerEventArgs>(Game_PlayerMoved);
            this.KeyDown += new KeyEventHandler(KeyCheck);
            this.KeyPreview = true;
            _model.GameAdvanced += new EventHandler<LabyrinthEventArgs>(Game_GameAdvanced);
            _model.GameOver += new EventHandler<LabyrinthEventArgs>(Game_GameOver);
            GenerateTable();
            SetupMenus();
            _model.FirsPosition();
            // idõzítõ létrehozása
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(Timer_Tick);

            // játéktábla és menük inicializálása



            _timer.Start();

        }
        private void ClearAll()
        {
            for (Int32 i = 0; i < _model.Table.Size; i++)
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    _LabelGrid[i, j].BackColor = Color.Gray;
                }
        }

        private void GenerateTable()
        {
            if (_LabelGrid != null)
            {
                foreach (var label in _LabelGrid)
                {
                    if (label != null)
                        Controls.Remove(label);
                }
            }
            _LabelGrid = new Label[_model.Table.Size, _model.Table.Size];
            for (Int32 i = 0; i < _model.Table.Size; i++)
                for (Int32 j = 0; j < _model.Table.Size; j++)
                {
                    _LabelGrid[i, j] = new Label();
                    _LabelGrid[i, j].Text = "";
                    _LabelGrid[i, j].BorderStyle = BorderStyle.Fixed3D;
                    _LabelGrid[i, j].Location = new Point(5 + 25 * j, 35 + 25 * i); // elhelyezkedés
                    _LabelGrid[i, j].Size = new Size(25, 25); // méret
                    _LabelGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                    //_LabelGrid[i, j].Enabled = false; // kikapcsolt állapot
                    //_LabelGrid[i, j].TabIndex = 100 + i * _model.Table.Size + j; // a gomb számát a TabIndex-ben tároljuk
                    _LabelGrid[i, j].FlatStyle = FlatStyle.Flat;
                    _LabelGrid[i, j].BackColor = Color.Gray;
                    // lapított stípus
                    //_LabelGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    // közös eseménykezelõ hozzárendelése minden gombhoz

                    Controls.Add(_LabelGrid[i, j]);
                    // felvesszük az ablakra a gombot
                }
        }

        private void Game_PlayerMoved(object? sender, LabyrinthPlayerEventArgs e)
        {
            ClearAll();
            List<Tuple<Int32, Int32>> coord = e.VisibleCoords;
            foreach (Tuple<Int32, Int32> tuple in coord)
            {

                if (_model.Table[tuple.Item1, tuple.Item2])
                {
                    _LabelGrid[tuple.Item1, tuple.Item2].BackColor = Color.Red;
                }
                else if (tuple.Item1 == _model.player.X && tuple.Item2 == _model.player.Y)
                {
                    _LabelGrid[tuple.Item1, tuple.Item2].BackColor = Color.Blue;
                }
                else
                {
                    _LabelGrid[tuple.Item1, tuple.Item2].BackColor = Color.Green;
                }

            }


        }
        private void KeyCheck(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case (Keys.W):
                    _model.Step(Direction.Up); break;
                case (Keys.D):
                    _model.Step(Direction.Right); break;
                case (Keys.S):
                    _model.Step(Direction.Down); break;
                case (Keys.A):
                    _model.Step(Direction.Left); break;


            }
        }

        private void Game_GameAdvanced(Object? sender, LabyrinthEventArgs e)
        {
            _timeLabel.Text = TimeSpan.FromSeconds(e.GameTime).ToString("g");

            // játékidõ frissítése
        }
        private void Timer_Tick(Object? sender, EventArgs e)
        {
            _model.AdvanceTime(); // játék léptetése
        }

        private void Game_GameOver(Object? sender, LabyrinthEventArgs e)
        {
            _timer.Stop();


            if (e.IsWon) // gyõzelemtõl függõ üzenet megjelenítése
            {
                MessageBox.Show("Gratulálok, gyõztél!" + Environment.NewLine +

                                TimeSpan.FromSeconds(e.GameTime).ToString("g") + " ideig játszottál.",
                                "Labirintus játék",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }

        }

        private void _newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _model.NewGame();
            if (_model.GameDifficulty == GameDifficulty.Easy)
            {
                this.Width = 280;
                
                this.Height = 400;
            }else if(_model.GameDifficulty == GameDifficulty.Medium)
            {
                this.Width = 300;
                this.Height = 400;
            }
            else if(_model.GameDifficulty == GameDifficulty.Hard)
            {
                this.Width = 350;
                this.Height = 400;
            }
            GenerateTable();
            SetupMenus();
            _model.FirsPosition();
            _timer.Start();
        }

        private void _QuitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();

            // megkérdezzük, hogy biztos ki szeretne-e lépni
            if (MessageBox.Show("Biztosan ki szeretne lépni?", "Labyrintus játék", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // ha igennel válaszol
                Close();
            }
            else
            {
                if (restartTimer)
                    _timer.Start();
            }
        }

        private void _PauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();

            // megkérdezzük, hogy biztos ki szeretne-e lépni
            if (MessageBox.Show("Játék szünetel", "Labirintus játék", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (restartTimer)
                    _timer.Start();
            }

        }
        private void SetupMenus()
        {
            _EasyToolStripMenuItem.Checked = (_model.GameDifficulty == GameDifficulty.Easy);
            _MediumToolStripMenuItem.Checked = (_model.GameDifficulty == GameDifficulty.Medium);
            HardToolStripMenuItem.Checked = (_model.GameDifficulty == GameDifficulty.Hard);
        }
        private void _EasyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _model.GameDifficulty = GameDifficulty.Easy;
        }

        private void _MediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _model.GameDifficulty = GameDifficulty.Medium;
        }

        private void HardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _model.GameDifficulty = GameDifficulty.Hard;
        }
    }
}
