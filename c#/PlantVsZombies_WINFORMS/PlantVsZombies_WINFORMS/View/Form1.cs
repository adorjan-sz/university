using PlantVsZombies.Model;
using PlantVsZombies_WINFORMS.View;
using System.Windows.Forms;

namespace PlantVsZombies_WINFORMS
{
    public partial class Form1 : Form
    {
        private GameModel gameModel = null!;
        private Field[,] fields = null!;
        private System.Windows.Forms.Timer timer = null!;
        public Form1()
        {
            InitializeComponent();

            gameModel = new GameModel();

            gameModel.GameAdvanced += new EventHandler(Game_GameAdvanced);
            gameModel.PeaShooterPlaced += new EventHandler(Game_PeashooterPlaced);
            gameModel.GameOver += new EventHandler(GameOver);

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(Timer_Tick);

            GenerateBoard();
            timer.Start();
        }

        private void GameOver(object? sender, EventArgs e)
        {
            timer.Stop();

            MessageBox.Show("Zombie is in the house!", "Game Over", MessageBoxButtons.OK);
        }

        private void Game_PeashooterPlaced(object? sender, EventArgs e)
        {
            RefreshGame();
        }

        private void Game_GameAdvanced(object? sender, EventArgs e)
        {
            RefreshGame();

            sunsText.Text = gameModel.Suns.ToString();
            timeText.Text = TimeSpan.FromSeconds(gameModel.GameTime).ToString("g");
        }

        private void RefreshGame()
        {
            foreach (Field field in fields)
            {
                if (gameModel.Board.Owners[field.X, field.Y] == "zombie")
                {
                    field.ChangeToZombieColor();
                }
                else if (gameModel.Board.Owners[field.X, field.Y] == "none")
                {
                    field.ChangeToBlankColor();
                }
                else
                {
                    field.ChangeToPeashooterColor();
                }
            }

            boardPanel.Refresh();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            gameModel.AdvanceTime();
        }

        private void GenerateBoard()
        {
            boardPanel.Controls.Clear();

            fields = new Field[5, 10];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    fields[i, j] = new Field(i, j);
                    fields[i, j].X = i;
                    fields[i, j].Y = j;
                    fields[i, j].ButtonClicked += new EventHandler(CheckPlacement);
                    boardPanel.Controls.Add(fields[i, j]);
                }
            }
        }

        private void CheckPlacement(object? sender, EventArgs e)
        {
            if (sender is Field field)
            {
                gameModel.Step(field.X, field.Y);
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
        }
    }
}