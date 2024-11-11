using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Galaga.Model
{
    /// <summary>
    ///     Manages the Galaga game play.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private const int PlayerShotDelay = 500;
        private const double PlayerOffsetFromBottom = 30;
        private const int MaxPlayerLives = 3;

        private readonly Canvas canvas;
        private readonly double canvasHeight;
        private readonly double canvasWidth;

        private Player player;
        private readonly EnemyManager enemyManager;
        private readonly BulletManager bulletManager;
        private readonly Ticker ticker;
        private readonly TextBlock scoreTextBlock;
        private readonly TextBlock gameOverBlock;
        private readonly TextBlock playerLivesTextBlock;

        #endregion

        #region Properties

        /// <summary>
        ///     Players Score in the game.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        ///     Players Lives in the game.
        /// </summary>
        public int PlayerLives { get; private set; } = MaxPlayerLives;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        public GameManager(Canvas canvas, TextBlock scoreTextBlock, TextBlock gameOverBlock, TextBlock playerLivesTextBlock)
        {
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));

            this.canvas = canvas;
            this.canvasHeight = canvas.Height;
            this.canvasWidth = canvas.Width;

            this.scoreTextBlock = scoreTextBlock;
            this.gameOverBlock = gameOverBlock;
            this.playerLivesTextBlock = playerLivesTextBlock;

            this.ticker = new Ticker();
            this.ticker.Tick += this.timer_Tick;
            this.ticker.Start();

            this.bulletManager = new BulletManager(canvas);
            this.enemyManager = new EnemyManager(this, canvas, this.bulletManager);

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
        public async void PlayerShoot()
        {
            if (!this.player.CanShoot)
            {
                return;
            }

            var bullet = this.player.Shoot();
            if (bullet != null)
            {
                this.bulletManager.AddPlayerBullet(bullet);
            }

            this.player.CanShoot = false;

            await Task.Delay(PlayerShotDelay);

            this.player.CanShoot = true;
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

            this.checkIfPlayerShotEnemy();
            this.checkIfEnemyShotPlayer();
        }

        private void checkIfPlayerShotEnemy()
        {
            var bullets = this.bulletManager.PlayerBullets;
            var bulletsToRemove = new List<Bullet>();

            foreach (var bullet in bullets)
            {
                if (this.enemyManager.CheckIfPlayerShotEnemy(bullet))
                {
                    bulletsToRemove.Add(bullet);
                }
            }
            
            foreach (var bullet in bulletsToRemove)
            {
                this.bulletManager.RemovePlayerBullet(bullet);
            }
        }

        private void checkIfEnemyShotPlayer()
        {
            var bullets = this.bulletManager.EnemyBullets;

            var bulletToRemove = new Bullet();

            foreach (var enemyBullet in bullets)
            {
                if (this.bulletManager.IsCollisionWithPlayer(enemyBullet, this.player))
                {
                    bulletToRemove = enemyBullet;
                    this.updatePlayerLives();
                }
            }

            this.bulletManager.RemoveEnemyBullet(bulletToRemove);
        }

        private void updatePlayerLives()
        {
            if (this.PlayerLives > 0)
            {
                this.PlayerLives--;
                this.playerLivesTextBlock.Text = "Lives: " + this.PlayerLives;
            }

            if (this.PlayerLives == 0)
            {
                this.endGame();
            }
        }

        private void endGame()
        {
            this.canvas.Children.Remove(this.player.Sprite);
            this.ticker.Stop();
            this.displayGameLose();
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