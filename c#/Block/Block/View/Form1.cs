using Accessibility;
using Block.Model;
using Block.Model.persistence;
using System.Windows.Forms;
using System;
using System.Drawing;


namespace Block
{
    public partial class Form1 : Form
    {
        private BlockModel _model;
        private Button[,] ButtonGrid;
        private Button[,] CurrentGrid;
        private System.Windows.Forms.OpenFileDialog _openFileDialog = new System.Windows.Forms.OpenFileDialog();
        private System.Windows.Forms.SaveFileDialog _saveFileDialog = new System.Windows.Forms.SaveFileDialog();

        public Form1()
        {

            InitializeComponent();
            _model = new BlockModel(new DataAcces());
            ButtonGrid = new Button[4, 4];
            for (int i = 0;i<4; i++)
            {
                for (int j = 0; j < 4; j++)
                { int x = i;
                    int y = j;
                    Button b = new Button();
                    b.Size = new Size(50, 50);
                    b.BackColor = Color.Green;
                    b.Location = new Point(5 + 50* j, 5 + 50 * i);
                    b.Click += new EventHandler((sender, e) => { _model.SetBlock(x,y);
                        
                    });
                    flowLayoutPanel1.Controls.Add(b);
                    ButtonGrid[i, j] = b;
                }
            }
            CurrentGrid = new Button[2, 2];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Button b = new Button();
                    b.Size = new Size(50, 50);
                    b.BackColor = Color.White;
                    b.Location = new Point(5 + 50 * j, 5 + 50 * i);
                    flowLayoutPanel2.Controls.Add(b);
                    CurrentGrid[i, j] = b;
                }
            }

            _model.TableChanged += new EventHandler<TableEventArgs>(UpdateTable);
            _model.CurrentChanged += new EventHandler<CurrentEventArgs>(UpdateCurrent);
            _model.GameOver += new EventHandler<GameOverEventArgs>(GameOver);
            _model.ScoreChanged += new EventHandler<ScoreEventArgs>((sender, e) => { label1.Text = e.Score.ToString(); });
            újJátékToolStripMenuItem.Click += new EventHandler((sender, e) => { _model.NewGame(); });
            mentésToolStripMenuItem.Click += new EventHandler(MenuFileSaveGame_Click);
            betöltésToolStripMenuItem.Click += new EventHandler(MenuFileLoadGame_Click);
            _model.NewGame();
        }
        private void UpdateTable(object? sender, TableEventArgs e)
        {
           
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (e[i, j] == BlockType.N)
                    {

                        ButtonGrid[i, j].BackColor = Color.White;
                    }
                    else
                    {
                        ButtonGrid[i, j].BackColor = Color.Blue;
                    }
                }
            }
        }
        private void GameOver(object? sender, GameOverEventArgs e)
        {
            if (e.IsGameOver)
            {
                MessageBox.Show("Game Over"+$"\n{label1.Text} pontott szereztél");
            }
        }
        private async void MenuFileLoadGame_Click(Object? sender, EventArgs e)
        {
            

            if (_openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await _model.Load(_openFileDialog.FileName);
                    
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                
            }

            
        }
        private async void MenuFileSaveGame_Click(Object? sender, EventArgs e)
        {
           

            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    await _model.Save(_saveFileDialog.FileName);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            
        }

        private void UpdateCurrent(object? sender, CurrentEventArgs e) {

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {

                    CurrentGrid[i,j].BackColor= Color.White;
                }
            }
                    if (e.Current == BlockType.V)
                    {
                        CurrentGrid[0, 0].BackColor = Color.Blue;
                        CurrentGrid[0+1, 0].BackColor = Color.Blue;
                    }
                    else if(e.Current == BlockType.H)
                    {
                        CurrentGrid[1, 0].BackColor = Color.Blue;
                        CurrentGrid[1, 0 + 1].BackColor = Color.Blue;
                    }
                    else if(e.Current == BlockType.L)
                    {
                        CurrentGrid[0, 0].BackColor = Color.Blue;
                        CurrentGrid[0 + 1, 0].BackColor = Color.Blue;
                        CurrentGrid[0 + 1, 0+1].BackColor = Color.Blue;
                    }
                    else if (e.Current == BlockType.T)
                    {
                        CurrentGrid[0, 0].BackColor = Color.Blue;
                        CurrentGrid[0, 0+1].BackColor = Color.Blue;
                        CurrentGrid[0 + 1, 0 + 1].BackColor = Color.Blue;
                    }
                
            
        }
    }
}
