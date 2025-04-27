using ModelAndPersistence.Model;
using ModelAndPersistence.Persistence;
namespace LocatorWinForms
{
    public partial class Form1 : Form
    {
        #region Fields

        private GameModel _model; // játékmodell
        private Button[,] _buttonGrid = null!; // gombrács

        #endregion

        #region Constructors

        /// <summary>
        /// Játékablak példányosítása.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            // adatelérés példányosítása
            DataAccess _dataAccess = new DataAccess();

            // modell létrehozása és az eseménykezelõk társítása
            _model = new GameModel(_dataAccess);
            _model.TableChanged += new EventHandler<TableEventArgs>(Update);
            _model.GameOver += new EventHandler<GameOverEventArgs>(Game_GameOver);

            // játéktábla és menük inicializálása
            _buttonGrid = new Button[_model.Size, _model.Size];
            for (Int32 i = 0; i < _model.Size; i++)
                for (Int32 j = 0; j < _model.Size; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(5 + 50 * j, 35 + 50 * i); // elhelyezkedés
                    _buttonGrid[i, j].Size = new Size(50, 50); // méret
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betûtípus
                    _buttonGrid[i, j].Enabled = true; // kikapcsolt állapot
                    _buttonGrid[i, j].TabIndex = 100 + i * _model.Size + j; // a gomb számát a TabIndex-ben tároljuk
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    // közös eseménykezelõ hozzárendelése minden gombhoz
                    //_buttonGrid[i, j].BackColor = Color.Black;
                    //if (!e.table.GetVisible(i, j)) { _buttonGrid[i, j].BackColor = Color.Black; }
                    Controls.Add(_buttonGrid[i, j]);
                    // felvesszük az ablakra a gombot
                }


            // új játék indítása
            _model.NewGame();
            
        }
        private void Update(object? sender,TableEventArgs e)
        {
            
            for (Int32 i = 0; i < _model.Size; i++)
                for (Int32 j = 0; j < _model.Size; j++)
                {
                    
                    
                    if (e.table.Get(i, j))
                    {
                        _buttonGrid[i, j].BackColor = Color.Red;
                    }
                    else
                    {
                        _buttonGrid[i, j].BackColor = Color.White;
                    }
                    if (!e.table.GetVisible(i, j)) { _buttonGrid[i, j].BackColor = Color.Black; }
                    
                    // felvesszük az ablakra a gombot
                }
        }
        private void Game_GameOver(Object? sender, GameOverEventArgs e)
        {
           

            

            
                MessageBox.Show("Gratulálok, gyõztél!" + Environment.NewLine +
                                "Összesen " + e.BombCount+ " lépést tettél meg és " 
                                ,
                    "Sudoku játék",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
            _model.NewGame();
           
        }
        private void ButtonGrid_MouseClick(Object? sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {

                // a TabIndex-bõl megkapjuk a sort és oszlopot
                int x = (button.TabIndex - 100) / _model.Size;
                int y = (button.TabIndex - 100) % _model.Size;


                _model.SetBomb(x, y); // lépés a játékban
            }
        }
        #endregion
    }
}
