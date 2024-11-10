using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace Galaga.Model
{
    /// <summary>
    ///     Manages the Galaga game play.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private const int PlayerMaxBullets = 1;
        private const double PlayerOffsetFromBottom = 30;
        private readonly Canvas canvas;
        private readonly double canvasHeight;
        private readonly double canvasWidth;

        private Player player;
        private readonly EnemyManager enemyManager;
        private readonly Ticker ticker;
        private readonly IList<Bullet> playerBullets;
        private readonly TextBlock scoreTextBlock;
        private readonly TextBlock gameOverBlock;

        #endregion

        #region Properties

        /// <summary>
        ///     Players Score in the game.
        /// </summary>
        public int Score { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
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
        ///     Moves the player left.
        /// </summary>
        public void MovePlayerLeft()
        {
            if (this.player.X > this.player.SpeedX)
            {
                this.player.MoveLeft();
            }
        }

        /// <summary>
        ///     Moves the player right.
        /// </summary>
        public void MovePlayerRight()
        {
            if (this.player.X < this.canvasWidth - this.player.Width - this.player.SpeedX)
            {
                this.player.MoveRight();
            }
        }

        /// <summary>
        ///     Provides the ticker for the game.
        /// </summary>
        /// <returns>
        ///     The ticker being used.
        /// </returns>
        public Ticker GetTicker()
        {
            return this.ticker;
        }

        /// <summary>
        ///     Handles the player shooting.
        /// </summary>
        public void PlayerShoot()
        {
            if (this.playerBullets.Count >= PlayerMaxBullets)
            {
                return;
            }

            var bullet = this.player.Shoot();
            if (bullet != null)
            {
                this.playerBullets.Add(bullet);
                this.canvas.Children.Add(bullet.Sprite);
            }
        }

        /// <summary>
        ///     Handles the game timer.
        /// </summary>
        /// <param name="sender">
        ///     the sender of the event.
        /// </param>
        /// <param name="e">
        ///     the event arguments.
        /// </param>
        public void timer_Tick(object sender, object e)
        {
            if (this.enemyManager.totalEnemies == 0)
            {
                this.displayGameWin();
            }

            if (this.playerBullets.Count > 0)
            {
                var bullet = this.playerBullets[0];
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

            var bulletsToRemove = new List<Bullet>();

            foreach (var enemyBullet in this.enemyManager.enemyBullets)
            {
                if (this.IsCollision(enemyBullet))
                {
                    bulletsToRemove.Add(enemyBullet);
                    this.canvas.Children.Remove(this.player.Sprite);
                    this.ticker.Stop();
                    this.displayGameLose();
                }

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

        private bool IsCollision(Bullet bullet)
        {
            var bulletBox = bullet.GetBoundingBox();
            var playerBox = this.player.GetBoundingBox();

            return bulletBox.IntersectsWith(playerBox);
        }

        /// <summary>
        ///     Adds points to the player's score.
        /// </summary>
        /// <param name="points">
        ///     Amount of points to add.
        /// </param>
        public void AddScore(int points)
        {
            this.Score += points;
            this.updateScoreUI();
        }

        /// <summary>
        ///     Updates the score UI
        /// </summary>
        public void updateScoreUI()
        {
            this.scoreTextBlock.Text = "Score: " + this.Score;
        }

        /// <summary>
        ///     Displays the game over screen when player loses.
        /// </summary>
        public void displayGameLose()
        {
            this.gameOverBlock.Text = "Game Over! \n  You Lose";
        }

        /// <summary>
        ///     Displays the game win screen when player wins.
        /// </summary>
        public void displayGameWin()
        {
            this.gameOverBlock.Text = "Game Over! \n  You Win";
        }

        #endregion
    }
}