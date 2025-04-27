using ModelAndPersistence.Model;
using ModelAndPersistence.Persistence;
namespace PvsZWinForms
{
    public partial class Form1 : Form
    {
        private GameModel _model;
        private Button[,] _buttonGrid = null!;
        public Form1()
        {
            InitializeComponent();
            _model = new GameModel(new DataAccess());
            _model.TableUpdate += new EventHandler<TableChanged>(RefreshTable);
            _model.GameOver += new EventHandler(ItsGameOver);
            GenerateTable();
            _model.NewGame();
        }
        private void GenerateTable()
        {
            _buttonGrid = new Button[_model.Row, _model.Column];
            for (Int32 i = 0; i < _model.Row; i++)
            {
                for (Int32 j = 0; j < _model.Column; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(5 + 50 * j, 35 + 50 * i); // elhelyezked�s
                    _buttonGrid[i, j].Size = new Size(50, 50); // m�ret
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // bet�t�pus
                    _buttonGrid[i, j].Enabled = true; // kikapcsolt �llapot
                    _buttonGrid[i, j].TabIndex = 100 + i * _model.Row + j * _model.Column; // a gomb sz�m�t a TabIndex-ben t�roljuk
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lap�tott st�pus
                    _buttonGrid[i, j].BackColor = Color.White;
                    _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    // k�z�s esem�nykezel� hozz�rendel�se minden gombhoz

                    Controls.Add(_buttonGrid[i, j]);
                    // felvessz�k az ablakra a gombot
                }
            }
        }
        private void RefreshTable(Object? sender, TableChanged e)
        {
            for (Int32 i = 0; i < _model.Row; i++)
            {
                for (Int32 j = 0; j < _model.Column; j++)
                {
                    
                    _buttonGrid[i, j].BackColor = e.table.GetEntity(i,j).IsZombie && !e.table.GetEntity(i, j).IsEmpty ? Color.Red : Color.Green;
                    if(e.table.GetEntity(i, j).IsEmpty)
                        _buttonGrid[i,j].BackColor = Color.White;


                }
            }
        }
        private void ButtonGrid_MouseClick(Object? sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {

                // a TabIndex-b�l megkapjuk a sort �s oszlopot
                Int32 x = (button.TabIndex - 100) / _model.Row;
                Int32 y = (button.TabIndex - 100) % _model.Column;

                _model.SetPlant(x, y); // l�p�s a j�t�kban
            }
        }
        private void ItsGameOver(Object? sender, EventArgs e)
        {
            MessageBox.Show("Game Over");
        }

    }
}
