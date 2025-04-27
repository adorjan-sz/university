using ModelAndPersistence.Model;
using ModelAndPersistence.Persistence;
namespace LocatorWinForms
{
    public partial class Form1 : Form
    {
        #region Fields

        private GameModel _model; // j�t�kmodell
        private Button[,] _buttonGrid = null!; // gombr�cs

        #endregion

        #region Constructors

        /// <summary>
        /// J�t�kablak p�ld�nyos�t�sa.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            // adatel�r�s p�ld�nyos�t�sa
            DataAccess _dataAccess = new DataAccess();

            // modell l�trehoz�sa �s az esem�nykezel�k t�rs�t�sa
            _model = new GameModel(_dataAccess);
            _model.TableChanged += new EventHandler<TableEventArgs>(Update);
            _model.GameOver += new EventHandler<GameOverEventArgs>(Game_GameOver);

            // j�t�kt�bla �s men�k inicializ�l�sa
            _buttonGrid = new Button[_model.Size, _model.Size];
            for (Int32 i = 0; i < _model.Size; i++)
                for (Int32 j = 0; j < _model.Size; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(5 + 50 * j, 35 + 50 * i); // elhelyezked�s
                    _buttonGrid[i, j].Size = new Size(50, 50); // m�ret
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // bet�t�pus
                    _buttonGrid[i, j].Enabled = true; // kikapcsolt �llapot
                    _buttonGrid[i, j].TabIndex = 100 + i * _model.Size + j; // a gomb sz�m�t a TabIndex-ben t�roljuk
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lap�tott st�pus
                    _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    // k�z�s esem�nykezel� hozz�rendel�se minden gombhoz
                    //_buttonGrid[i, j].BackColor = Color.Black;
                    //if (!e.table.GetVisible(i, j)) { _buttonGrid[i, j].BackColor = Color.Black; }
                    Controls.Add(_buttonGrid[i, j]);
                    // felvessz�k az ablakra a gombot
                }


            // �j j�t�k ind�t�sa
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
                    
                    // felvessz�k az ablakra a gombot
                }
        }
        private void Game_GameOver(Object? sender, GameOverEventArgs e)
        {
           

            

            
                MessageBox.Show("Gratul�lok, gy�zt�l!" + Environment.NewLine +
                                "�sszesen " + e.BombCount+ " l�p�st tett�l meg �s " 
                                ,
                    "Sudoku j�t�k",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
            _model.NewGame();
           
        }
        private void ButtonGrid_MouseClick(Object? sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {

                // a TabIndex-b�l megkapjuk a sort �s oszlopot
                int x = (button.TabIndex - 100) / _model.Size;
                int y = (button.TabIndex - 100) % _model.Size;


                _model.SetBomb(x, y); // l�p�s a j�t�kban
            }
        }
        #endregion
    }
}
