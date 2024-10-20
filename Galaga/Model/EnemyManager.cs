using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Galaga.Model
{
    /// <summary>
    /// Manages the enemies in the game.
    /// </summary>
    public class EnemyManager
    {
        #region Data members

        private const double EnemyBuffer = 10;
        private readonly Canvas canvas;
        private readonly double canvasHeight;
        private readonly double canvasWidth;

        private IList<Enemy> Type_1_Enemies;
        private IList<Enemy> Type_2_Enemies;
        private IList<Enemy> Type_3_Enemies;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyManager"/> class.
        /// </summary>
        /// <param name="canvas">
        ///     The canvas to draw the enemies on.
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public EnemyManager(Canvas canvas)
        {
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));

            this.canvas = canvas;
            this.canvasHeight = canvas.Height;
            this.canvasWidth = canvas.Width;

            this.Type_1_Enemies = new List<Enemy>();
            this.Type_2_Enemies = new List<Enemy>();
            this.Type_3_Enemies = new List<Enemy>();

            this.initializeGame();
        }

        #endregion

        #region Methods

        private void initializeGame()
        {
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
                    for (int i = 0; i < numEnemies; i++)
                    {
                        var enemy = new Enemy(type);
                        this.Type_1_Enemies.Add(enemy);
                        this.canvas.Children.Add(enemy.Sprite);
                    }
                    break;

                case GlobalEnums.EnemySpriteType.TYPE2:
                    for (int i = 0; i < numEnemies; i++)
                    {
                        var enemy = new Enemy(type);
                        this.Type_2_Enemies.Add(enemy);
                        this.canvas.Children.Add(enemy.Sprite);
                    }
                    break;

                case GlobalEnums.EnemySpriteType.TYPE3:
                    for (int i = 0; i < numEnemies; i++)
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
            if (enemies.Count == 0) return;

            double totalRowWidth = enemies.Count * enemies[0].Width + (enemies.Count - 1) * EnemyBuffer;

            double startX = (this.canvasWidth / 2) - (totalRowWidth / 2);

            for (int i = 0; i < enemies.Count; i++)
            {
                double xPositioin = startX + i * (enemies[i].Width + EnemyBuffer);
                enemies[i].X = xPositioin;
                enemies[i].Y = yPosition;
            }
        }

        #endregion
    }
}