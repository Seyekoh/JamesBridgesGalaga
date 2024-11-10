using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Galaga.Model
{
    /// <summary>
    ///     Manages the enemies in the game.
    /// </summary>
    public class EnemyManager
    {
        #region Data members

        private const int EnemyType1Max = 3;
        private const int EnemyType2Max = 4;
        private const int EnemyType3Max = 4;
        private const int EnemyType4Max = 5;

        private const double EnemyToCanvasBuffer = 5;
        private const double EnemyToEnemyBuffer = 10;

        private const int EnemyFireDelayMin = 3000;
        private const int EnemyFireDelayMax = 5000;

        private readonly Canvas canvas;
        private readonly double canvasWidth;

        private readonly GameManager gameManager;
        private Ticker ticker;
        private readonly Random random = new Random();

        private readonly IList<Enemy> Enemies;

        #endregion

        #region Properties

        /// <summary>
        ///     Property to get the total number of enemies.
        /// </summary>
        public int totalEnemies => this.Enemies.Count;

        /// <summary>
        ///     Property to keep track of enemy bullets.
        /// </summary>
        public IList<Bullet> enemyBullets { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemyManager" /> class.
        /// </summary>
        /// <param name="gameManager">
        ///     The game manager being used.
        /// </param>
        /// <param name="canvas">
        ///     The canvas to draw the enemies on.
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public EnemyManager(GameManager gameManager, Canvas canvas)
        {
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));

            this.canvas = canvas;
            this.canvasWidth = canvas.Width;

            this.gameManager = gameManager;

            this.enemyBullets = new List<Bullet>();

            this.Enemies = new List<Enemy>();

            this.initializeGame();
        }

        #endregion

        #region Methods

        private void initializeGame()
        {
            this.ticker = this.gameManager.GetTicker();
            this.ticker.Tick += this.timer_Tick;
            this.createAndPlaceEnemies();
        }

        private void createAndPlaceEnemies()
        {
            this.createEnemies(EnemyType1Max, GlobalEnums.EnemySpriteType.TYPE1);
            this.createEnemies(EnemyType2Max, GlobalEnums.EnemySpriteType.TYPE2);
            this.createEnemies(EnemyType3Max, GlobalEnums.EnemySpriteType.TYPE3);
            this.createEnemies(EnemyType4Max, GlobalEnums.EnemySpriteType.TYPE4);

            this.placeEnemies();
        }

        private void createEnemies(int numEnemies, GlobalEnums.EnemySpriteType type)
        {
            for (var i = 0; i < numEnemies; i++)
            {
                var enemy = new Enemy(type);
                this.Enemies.Add(enemy);
                this.canvas.Children.Add(enemy.Sprite);
            }
        
        }

        private void placeEnemies()
        {
            double[] rowHeights = { 50, 100, 150, 200 };

            this.placeRowEnemies(this.Enemies, GlobalEnums.EnemySpriteType.TYPE1, rowHeights[3]);

            this.placeRowEnemies(this.Enemies, GlobalEnums.EnemySpriteType.TYPE2, rowHeights[2]);

            this.placeRowEnemies(this.Enemies, GlobalEnums.EnemySpriteType.TYPE3, rowHeights[1]);

            this.placeRowEnemies(this.Enemies, GlobalEnums.EnemySpriteType.TYPE4, rowHeights[0]);
        }

        private void placeRowEnemies(IList<Enemy> enemies, GlobalEnums.EnemySpriteType type, double yPosition)
        {
            var rowEnemies = enemies.Where(e => e.type == type).ToList();
            if (rowEnemies.Count == 0)
            {
                return;
            }

            var totalRowWidth = rowEnemies.Count * rowEnemies[0].Width + (rowEnemies.Count - 1) * EnemyToEnemyBuffer;
            var startX = this.canvasWidth / 2 - totalRowWidth / 2;

            for (var i = 0; i < rowEnemies.Count; i++)
            {
                var xPosition = startX + i * (rowEnemies[i].Width + EnemyToEnemyBuffer);
                rowEnemies[i].X = xPosition;
                rowEnemies[i].Y = yPosition;

                rowEnemies[i].InitialX = xPosition;
            }
        }

        private async void timer_Tick(object sender, object e)
        {
            this.updateEnemyMovement(this.Enemies);

            this.moveEnemyBullets();

            this.letEnemyShoot();
            await Task.Delay(this.random.Next(EnemyFireDelayMin, EnemyFireDelayMax));
        }

        private void letEnemyShoot()
        {
            var shootingEnemies = this.Enemies
                .Where(enemy => enemy.type == GlobalEnums.EnemySpriteType.TYPE3 ||
                                enemy.type == GlobalEnums.EnemySpriteType.TYPE4)
                .ToList();

            foreach (var enemy in shootingEnemies)
            {
                if (this.random.Next(0, 10) >= 9) // 10% chance to shoot
                {
                    var bullet = enemy.Shoot();
                    if (bullet != null)
                    {
                        this.enemyBullets.Add(bullet);
                        this.canvas.Children.Add(bullet.Sprite);
                    }
                }
            }
        }

        private void moveEnemyBullets()
        {
            foreach (var bullet in this.enemyBullets)
            {
                bullet.MoveDown();
            }
        }

        private void updateEnemyMovement(IList<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                var x = Canvas.GetLeft(enemy.Sprite);

                if (enemy.MovingRight)
                {
                    Canvas.SetLeft(enemy.Sprite, x + enemy.SpeedX);

                    if (x >= enemy.InitialX + EnemyToCanvasBuffer * enemy.SpeedX)
                    {
                        enemy.MovingRight = false;
                    }
                }
                else
                {
                    Canvas.SetLeft(enemy.Sprite, x - enemy.SpeedX);

                    if (x <= enemy.InitialX - EnemyToCanvasBuffer * enemy.SpeedX)
                    {
                        enemy.MovingRight = true;
                    }
                }
            }
        }

        /// <summary>
        ///     Checks if a player's bullet has collided with an enemy.
        /// </summary>
        /// <param name="bullet">
        ///     The player's bullet.
        /// </param>
        /// <returns>
        ///     True if the bullet has collided with an enemy, false otherwise.
        /// </returns>
        public bool CheckBulletCollision(Bullet bullet)
        {
            foreach (var enemy in this.Enemies)
            {
                if (this.IsCollision(bullet, enemy))
                {
                    this.RemoveEnemy(enemy);
                    return true;
                }
            }

            return false;
        }

        private bool IsCollision(Bullet bullet, Enemy enemy)
        {
            var bulletBox = bullet.GetBoundingBox();
            var enemyBox = enemy.GetBoundingBox();

            return bulletBox.IntersectsWith(enemyBox);
        }

        private void RemoveEnemy(Enemy enemy)
        {
            this.canvas.Children.Remove(enemy.Sprite);
            this.Enemies.Remove(enemy);

            var points = 0;
            points += enemy.Score;

            this.gameManager.AddScore(points);
        }

        #endregion
    }
}