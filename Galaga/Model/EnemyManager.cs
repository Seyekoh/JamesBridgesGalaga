using System;
using System.Collections.Generic;
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

        private const double EnemyBuffer = 10;

        /// <summary>
        ///     List of all active enemy bullets.
        /// </summary>
        public IList<Bullet> enemyBullets;

        private readonly Canvas canvas;
        private readonly double canvasWidth;

        private readonly GameManager gameManager;
        private Ticker ticker;
        private readonly Random random = new Random();

        private readonly IList<Enemy> Type_1_Enemies;
        private readonly IList<Enemy> Type_2_Enemies;
        private readonly IList<Enemy> Type_3_Enemies;

        #endregion

        #region Properties

        /// <summary>
        ///     Property to get the total number of enemies.
        /// </summary>
        public int totalEnemies => this.Type_1_Enemies.Count + this.Type_2_Enemies.Count + this.Type_3_Enemies.Count;

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

            this.Type_1_Enemies = new List<Enemy>();
            this.Type_2_Enemies = new List<Enemy>();
            this.Type_3_Enemies = new List<Enemy>();

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
            this.createEnemies(2, GlobalEnums.EnemySpriteType.TYPE1);
            this.createEnemies(3, GlobalEnums.EnemySpriteType.TYPE2);
            this.createEnemies(4, GlobalEnums.EnemySpriteType.TYPE3);

            this.placeEnemies();
        }

        private void createEnemies(int numEnemies, GlobalEnums.EnemySpriteType type)
        {
            switch (type)
            {
                case GlobalEnums.EnemySpriteType.TYPE1:
                    for (var i = 0; i < numEnemies; i++)
                    {
                        var enemy = new Enemy(type);
                        this.Type_1_Enemies.Add(enemy);
                        this.canvas.Children.Add(enemy.Sprite);
                    }

                    break;

                case GlobalEnums.EnemySpriteType.TYPE2:
                    for (var i = 0; i < numEnemies; i++)
                    {
                        var enemy = new Enemy(type);
                        this.Type_2_Enemies.Add(enemy);
                        this.canvas.Children.Add(enemy.Sprite);
                    }

                    break;

                case GlobalEnums.EnemySpriteType.TYPE3:
                    for (var i = 0; i < numEnemies; i++)
                    {
                        var enemy = new Enemy(type);
                        this.Type_3_Enemies.Add(enemy);
                        this.canvas.Children.Add(enemy.Sprite);
                    }

                    break;
            }
        }

        private void placeEnemies()
        {
            double[] rowHeights = { 50, 100, 150 };

            this.placeRowEnemies(this.Type_1_Enemies, rowHeights[2]);

            this.placeRowEnemies(this.Type_2_Enemies, rowHeights[1]);

            this.placeRowEnemies(this.Type_3_Enemies, rowHeights[0]);
        }

        private void placeRowEnemies(IList<Enemy> enemies, double yPosition)
        {
            if (enemies.Count == 0)
            {
                return;
            }

            var totalRowWidth = enemies.Count * enemies[0].Width + (enemies.Count - 1) * EnemyBuffer;

            var startX = this.canvasWidth / 2 - totalRowWidth / 2;

            for (var i = 0; i < enemies.Count; i++)
            {
                var xPosition = startX + i * (enemies[i].Width + EnemyBuffer);
                enemies[i].X = xPosition;
                enemies[i].Y = yPosition;

                enemies[i].InitialX = xPosition;
            }
        }

        private async void timer_Tick(object sender, object e)
        {
            this.updateEnemyMovement(this.Type_1_Enemies);
            this.updateEnemyMovement(this.Type_2_Enemies);
            this.updateEnemyMovement(this.Type_3_Enemies);

            foreach (var bullet in this.enemyBullets)
            {
                bullet.MoveDown();
            }

            foreach (var enemy in this.Type_3_Enemies)
            {
                if (this.random.Next(0, 10) >= 9)
                {
                    var bullet = enemy.Shoot();
                    if (bullet != null)
                    {
                        this.enemyBullets.Add(bullet);
                        this.canvas.Children.Add(bullet.Sprite);
                    }
                }
            }

            await Task.Delay(this.random.Next(3000, 5000));
        }

        private void updateEnemyMovement(IList<Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                var x = Canvas.GetLeft(enemy.Sprite);

                if (enemy.MovingRight)
                {
                    Canvas.SetLeft(enemy.Sprite, x + enemy.SpeedX);

                    if (x >= enemy.InitialX + 5 * enemy.SpeedX)
                    {
                        enemy.MovingRight = false;
                    }
                }
                else
                {
                    Canvas.SetLeft(enemy.Sprite, x - enemy.SpeedX);

                    if (x <= enemy.InitialX - 5 * enemy.SpeedX)
                    {
                        enemy.MovingRight = true;
                    }
                }
            }
        }

        /// <summary>
        ///     Checks if a bullet has collided with an enemy.
        /// </summary>
        /// <param name="bullet">
        ///     The player's bullet.
        /// </param>
        /// <returns>
        ///     True if the bullet has collided with an enemy, false otherwise.
        /// </returns>
        public bool CheckBulletCollision(Bullet bullet)
        {
            foreach (var enemy in this.Type_1_Enemies)
            {
                if (this.IsCollision(bullet, enemy))
                {
                    this.RemoveEnemy(enemy);
                    return true;
                }
            }

            foreach (var enemy in this.Type_3_Enemies)
            {
                if (this.IsCollision(bullet, enemy))
                {
                    this.RemoveEnemy(enemy);
                    return true;
                }
            }

            foreach (var enemy in this.Type_2_Enemies)
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

            var points = 0;
            var type = enemy.type;
            switch (type)
            {
                case GlobalEnums.EnemySpriteType.TYPE1:
                    this.Type_1_Enemies.Remove(enemy);
                    points = 10;
                    break;
                case GlobalEnums.EnemySpriteType.TYPE2:
                    this.Type_2_Enemies.Remove(enemy);
                    points = 20;
                    break;
                case GlobalEnums.EnemySpriteType.TYPE3:
                    this.Type_3_Enemies.Remove(enemy);
                    points = 30;
                    break;
            }

            this.gameManager.AddScore(points);
        }

        #endregion
    }
}