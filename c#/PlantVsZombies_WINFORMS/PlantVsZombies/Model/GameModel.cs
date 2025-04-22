using PlantVsZombies.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantVsZombies.Model
{
    public class GameModel
    {
        private Board board;
        private int suns;
        private int gameTime;
        private bool gameOver;
        public int Suns
        {
            get { return suns; }
            set { suns = value; }
        }

        public int GameTime
        {
            get { return gameTime; }
            set { gameTime = value; }
        }
        public Board Board { get { return board; } }

        public event EventHandler? GameAdvanced;
        public event EventHandler? PeaShooterPlaced;
        public event EventHandler? GameOver;

        public GameModel()
        {
            gameOver = false;
            board = new Board();
            suns = 75;

            CreateBoard();
        }

        private void CreateBoard()
        {
            for(int i = 0;i< 5; i++)
            {
                for(int j = 0;j<10;j++)
                {
                    board.Owners[i, j] = "none";
                }
            }
        }

        public void AdvanceTime()
        {
            if (!gameOver)
            {
                gameTime++;

                AdvanceZombies();

                if (gameTime % 4 == 0)
                {
                    PeashootersShoot();
                    suns += 25;
                }

                SpawnRandomZombie();

                OnGameAdvanced();
            }
        }

        private void PeashootersShoot()
        {
            for (int i = 0; i < 5; i++)
            {
                int j = 0;
                while (j < 10)
                {
                    if (board.Owners[i, j] == "peashooter")
                    {
                        break;
                    }
                    j++;
                }
                bool zombiehit = false;

                while(j<10 && !zombiehit)
                {
                    if (board.Owners[i, j] == "zombie")
                    {
                        board.Owners[i, j] = "none";
                        zombiehit = true;
                    }
                    j++;
                }
            }
        }

        private void AdvanceZombies()
        {
            for(int i = 0;i<5 ;i++)
            {
                for(int j = 0;j<10;j++)
                {
                    try
                    {
                        if (board.Owners[i, j] == "zombie" && board.Owners[i, j - 1] != "peashooter")
                        {
                            board.Owners[i, j] = "none";
                            board.Owners[i, j - 1] = "zombie";
                        }
                        else if (board.Owners[i, j] == "zombie" && board.Owners[i, j - 1] == "peashooter")
                        {
                            board.Owners[i, j] = "zombie";
                            board.Owners[i, j - 1] = "deadpeashooter";
                        }
                        else if (board.Owners[i, j] == "zombie" && board.Owners[i, j - 1] == "deadpeashooter")
                        {
                            board.Owners[i, j] = "none";
                            board.Owners[i, j - 1] = "zombie";
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        GameOver?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        private void SpawnRandomZombie()
        {
            Random rn = new Random();
            int chance = rn.Next(0, 10);
            if (chance < 3)
            {
                int x = rn.Next(0, 5);
                while (board.Owners[x, 9] == "zombie")
                {
                    x = rn.Next(0, 5);
                }
                board.Owners[x, 9] = "zombie";
            }
        }

        private void OnGameAdvanced()
        {
            GameAdvanced?.Invoke(this, EventArgs.Empty);
        }

        public void Step(int x, int y)
        {
            if (board.Owners[x,y] == "none" && suns >= 100)
            {
                suns -= 100;
                board.Owners[x, y] = "peashooter";
            }

            PeaShooterPlaced?.Invoke(this, EventArgs.Empty);
        }
    }
}
