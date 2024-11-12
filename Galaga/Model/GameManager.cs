using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Galaga.Model
{
    /// <summary>
    ///     Manages the Galaga game play.
    /// </summary>
    public class GameManager
    {
        #region Events

        /// <summary>
        ///     Delegate for updating the score.
        /// </summary>
        /// <param name="newScore">
        ///     The new score to update to.
        /// </param>
        public delegate void UpdateScoreHandler(int newScore);

        /// <summary>
        ///     Delegate for updating the player lives.
        /// </summary>
        /// <param name="newLives">
        ///     The new number of lives to update to.
        /// </param>
        public delegate void UpdatePlayerLivesHandler(int newLives);

        /// <summary>
        ///     Delegate for displaying the game over message.
        /// </summary>
        /// <param name="typeOfGameOver">
        ///     The type of game over that occurred.
        /// </param>
        public delegate void DisplayGameOverHandler(GlobalEnums.GameOverType typeOfGameOver);
        //public delegate void AddSpriteHandler(UIElement sprite);
        //public delegate void RemoveSpriteHandler(UIElement sprite);

        /// <summary>
        ///     Event for updating the score.
        /// </summary>
        public event UpdateScoreHandler OnScoreUpdated;

        /// <summary>
        ///     Event for updating the player lives.
        /// </summary>
        public event UpdatePlayerLivesHandler OnPlayerLivesUpdated;

        /// <summary>
        ///     Event for displaying the game over message.
        /// </summary>
        public event DisplayGameOverHandler OnGameOver;
        //public event AddSpriteHandler OnSpriteAdded;
        //public event RemoveSpriteHandler OnSpriteRemoved;



        #endregion

        #region Data members

        private const double TimerInterval = 16;
        private const double PlayerOffsetFromBottom = 30;
        private const int PlayerShotDelay = 500;
        private const int MaxPlayerLives = 3;

        private readonly Canvas canvas;
        private readonly double canvasHeight;
        private readonly double canvasWidth;

        private Player player;

        private readonly EnemyManager enemyManager;
        private readonly BulletManager bulletManager;
        private readonly DispatcherTimer timer = new DispatcherTimer();

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
        public GameManager(Canvas canvas, BulletManager bulletManager, EnemyManager enemyManager)
        {
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));

            this.canvas = canvas;
            this.canvasHeight = canvas.Height;
            this.canvasWidth = canvas.Width;

            this.timer.Interval = TimeSpan.FromMilliseconds(TimerInterval);
            this.timer.Tick += this.timer_Tick;
            this.timer.Start();

            this.bulletManager = bulletManager;
            this.enemyManager = enemyManager;

            this.enemyManager.OnScoreUpdated += this.UpdateScore;

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
        public void movePlayerLeft()
        {
            if (this.player.X > this.player.SpeedX)
            {
                this.player.MoveLeft();
            }
        }

        /// <summary>
        ///     Moves the player right.
        /// </summary>
        public void movePlayerRight()
        {
            if (this.player.X < this.canvasWidth - this.player.Width - this.player.SpeedX)
            {
                this.player.MoveRight();
            }
        }

        /// <summary>
        ///     Handles the player shooting.
        /// </summary>
        public async void playerShoot()
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
                this.winGame();
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
                this.bulletManager.RemovePlayerBulletAfterImpact(bullet);
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

            this.bulletManager.RemoveEnemyBulletAfterImpact(bulletToRemove);
        }

        private void updatePlayerLives()
        {
            if (this.PlayerLives > 0)
            {
                this.PlayerLives--;
                this.OnPlayerLivesUpdated?.Invoke(this.PlayerLives);
            }

            if (this.PlayerLives == 0)
            {
                this.loseGame();
            }
        }

        private void loseGame()
        {
            this.timer.Stop();
            this.canvas.Children.Remove(this.player.Sprite);
            this.OnGameOver?.Invoke(GlobalEnums.GameOverType.LOSE);
        }

        private void winGame()
        {
            this.timer.Stop();
            this.OnGameOver?.Invoke(GlobalEnums.GameOverType.WIN);
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
            this.OnScoreUpdated?.Invoke(this.Score);
        }

        private void UpdateScore(int points)
        {
            this.AddScore(points);
        }


        #endregion
    }
}