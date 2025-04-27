using Game.Model;
using Game.Persistence;
using System.Runtime.CompilerServices;
namespace RubikWinForms
{
    public partial class Form1 : Form
    {
        private Button[,] _buttonGrid = null!; // gombr�cs
        private GameModel _model = null!;
        public Form1()
        {
            InitializeComponent();
            _model = new GameModel(new DataAccess());
            _model.TableChanged += new EventHandler<TableChangedEventArgs>(UpdateTable);

            GenerateTable();
            _model.NewGame();

        }
        private void UpdateTable(object sender, TableChangedEventArgs e)
        {
            
            for (Int32 i = 0; i < _model.TableSize; i++)
                for (Int32 j = 0; j < _model.TableSize; j++)
                {
                    switch (e.table[i, j])
                    {
                        case Colour.G:
                            _buttonGrid[i, j].BackColor = Color.Green; 
                            break;
                        case Colour.R:
                            _buttonGrid[i, j].BackColor = Color.Red;
                            break;
                        case Colour.B:
                            _buttonGrid[i, j].BackColor = Color.Blue;
                            break;
                        case Colour.Y:
                            _buttonGrid[i, j].BackColor = Color.Yellow;
                            break;
                    }
                }
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
                    //_buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    // k�z�s esem�nykezel� hozz�rendel�se minden gombhoz
                    if (i == 0 && j == 0)
                    {
                        _buttonGrid[i, j].Enabled = true;
                        _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    }
                    else if (i == 0 && j == _model.TableSize - 1) {
                        _buttonGrid[i, j].Enabled = true;
                        _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);

                    }else if(i== _model.TableSize - 1 && j == 0)
                    {
                        _buttonGrid[i, j].Enabled = true;
                        _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);

                    }
                    else if(i== _model.TableSize - 1&& j == _model.TableSize - 1)
                    {
                        _buttonGrid[i, j].Enabled = true;
                        _buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);

                    }

                    Controls.Add(_buttonGrid[i, j]);
                    // felvessz�k az ablakra a gombot
                }

        }
        private void ButtonGrid_MouseClick(Object? sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {

                // a TabIndex-b�l megkapjuk a sort �s oszlopot
                int i = (button.TabIndex - 100) / _model.TableSize;
                int j = (button.TabIndex - 100) % _model.TableSize;
                _model.Rotate(i,j);
                //_model.Step(x, y); // l�p�s a j�t�kban
            }
        }
    }
}
