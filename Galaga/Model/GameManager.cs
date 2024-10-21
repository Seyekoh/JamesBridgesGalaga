using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace Galaga.Model
{
    /// <summary>
    /// Manages the Galaga game play.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private const double PlayerOffsetFromBottom = 30;
        private readonly Canvas canvas;
        private readonly double canvasHeight;
        private readonly double canvasWidth;

        private Player player;
        private EnemyManager enemyManager;
        private Ticker ticker;
        private IList<Bullet> playerBullets;
        private TextBlock scoreTextBlock;
        private TextBlock gameOverBlock;

        public int Score { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GameManager"/> class.
        /// </summary>
        public GameManager(Canvas canvas, TextBlock scoreTextBlock, TextBlock gameOverBlock)
        {
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));

            this.canvas = canvas;
            this.canvasHeight = canvas.Height;
            this.canvasWidth = canvas.Width;

            this.scoreTextBlock = scoreTextBlock;
            this.gameOverBlock = gameOverBlock;

            this.ticker = new Ticker();
            this.ticker.Tick += this.timer_Tick;
            this.ticker.Start();

            this.enemyManager = new EnemyManager(this, canvas);

            this.playerBullets = new List<Bullet>();
            this.Score = 0;

            this.initializeGame();
        }

        #endregion

        #region Methods

        private void initializeGame()
        {
            this.createAndPlacePlayer();
        }

        private void createAndPlacePlayer()
        {
            this.player = new Player();
            this.canvas.Children.Add(this.player.Sprite);

            this.placePlayerNearBottomOfBackgroundCentered();
        }

        private void placePlayerNearBottomOfBackgroundCentered()
        {
            this.player.X = this.canvasWidth / 2 - this.player.Width / 2.0;
            this.player.Y = this.canvasHeight - this.player.Height - PlayerOffsetFromBottom;
        }

        /// <summary>
        /// Moves the player left.
        /// </summary>
        public void MovePlayerLeft()
        {
            if (this.player.X > this.player.SpeedX)
            {
                this.player.MoveLeft();
            }
        }

        /// <summary>
        /// Moves the player right.
        /// </summary>
        public void MovePlayerRight()
        {
            if (this.player.X < this.canvasWidth - this.player.Width - this.player.SpeedX)
            {
                this.player.MoveRight();
            }
        }

        public Ticker GetTicker()
        {
            return this.ticker;
        }

        public void PlayerShoot()
        {
            if (this.playerBullets.Count >= 1)
            {
                return;
            }
            Bullet bullet = this.player.Shoot();
            if (bullet != null)
            {
                this.playerBullets.Add(bullet);
                this.canvas.Children.Add(bullet.Sprite);
            }
        }

        public void timer_Tick(object sender, object e)
        {
            if (this.enemyManager.totalEnemies == 0)
            {
                this.displayGameWin();
            }

            if (this.playerBullets.Count > 0)
            {
                Bullet bullet = this.playerBullets[0];
                bullet.MoveUp();

                if (this.enemyManager.CheckBulletCollision(bullet))
                {
                    this.canvas.Children.Remove(bullet.Sprite);
                    this.playerBullets.RemoveAt(0);
                }

                if (bullet.Y < 0)
                {
                    this.canvas.Children.Remove(bullet.Sprite);
                    this.playerBullets.RemoveAt(0);
                }
            }

            List<Bullet> bulletsToRemove = new List<Bullet>();

            foreach (var enemyBullet in this.enemyManager.enemyBullets)
            {
                if (this.IsCollision(enemyBullet, this.player))
                {
                    Debug.WriteLine("HIT!!!");
                    bulletsToRemove.Add(enemyBullet);
                    this.canvas.Children.Remove(this.player.Sprite);
                    this.ticker.Stop();
                    this.displayGameLose();
                }
                Debug.WriteLine("MISS!!!");

                if (enemyBullet.Y > this.canvasHeight)
                {
                    this.canvas.Children.Remove(enemyBullet.Sprite);
                    bulletsToRemove.Add(enemyBullet);
                }
            }

            foreach (var bullet in bulletsToRemove)
            {
                this.enemyManager.enemyBullets.Remove(bullet);
                this.canvas.Children.Remove(bullet.Sprite);
            }


        }

        private bool IsCollision(Bullet bullet, Player player)
        {
            Debug.WriteLine("Checking collision...");
            Rect bulletBox = bullet.GetBoundingBox();
            Rect playerBox = player.GetBoundingBox();

            Debug.WriteLine($"Bullet Box: {bulletBox}");
            Debug.WriteLine($"Payer Box: {playerBox}");

            return bulletBox.IntersectsWith(playerBox);
        }

        public void AddScore(int points)
        {
            this.Score += points;
            this.updateScoreUI(this.scoreTextBlock);
        }

        public void updateScoreUI(TextBlock scoreTextBlock)
        {
            scoreTextBlock.Text = "Score: " + this.Score.ToString();
        }

        public void displayGameLose()
        {
            this.gameOverBlock.Text = "Game Over! \n  You Lose" ;
        }

        public void displayGameWin()
        {
            this.gameOverBlock.Text = "Game Over! \n  You Win";
        }

        #endregion

    }
}
