using GameModel.Model;
using GameModel.Persistence;
namespace GameOfLifeWF
{
    public partial class Form1 : Form
    {
        private LifeGameModel _model;
        private Button[,] _buttonGrid = null!;
        private DataAccess data;

        public Form1()
        {
            InitializeComponent();
            data = new DataAccess();
            _model = new LifeGameModel(data);
            _model.TableChanged += new EventHandler<TableChangedEventArgs>(TableChanged);
            GenerateTable();
            _model.NewGame();
        }

        private void GenerateTable()
        {
            _buttonGrid = new Button[_model.TableSize, _model.TableSize];
            for (Int32 i = 0; i < _model.TableSize; i++)
                for (Int32 j = 0; j < _model.TableSize; j++)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(5 + 50 * j, 35 + 50 * i); // elhelyezked�s
                    _buttonGrid[i, j].Size = new Size(50, 50); // m�ret
                    _buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // bet�t�pus
                    _buttonGrid[i, j].Enabled = false; // kikapcsolt �llapot
                    _buttonGrid[i, j].TabIndex = 100 + i * _model.TableSize + j; // a gomb sz�m�t a TabIndex-ben t�roljuk
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lap�tott st�pus
                    _buttonGrid[i, j].BackColor = Color.White;
                   
                   _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    // k�z�s esem�nykezel� hozz�rendel�se minden gombhoz

                    Controls.Add(_buttonGrid[i, j]);
                    // felvessz�k az ablakra a gombot
                }
        }
        private void TableChanged(Object sender, TableChangedEventArgs e)
        {
            for (int i = 0; i < _model.TableSize; ++i)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    _buttonGrid[i,j].BackColor = _model.IsAlive(i,j) ? Color.Red : Color.White;
                    _buttonGrid[i, j].Enabled = true;
                }
            }
        }
        
        private void ButtonGrid_MouseClick(Object? sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {

                // a TabIndex-b�l megkapjuk a sort �s oszlopot
                int x = (button.TabIndex - 100) / _model.TableSize;
                int y = (button.TabIndex - 100) % _model.TableSize;
                MessageBox.Show("idk");
                _model.Set(x, y); // l�p�s a j�t�kban
            }
        }
    }
}
