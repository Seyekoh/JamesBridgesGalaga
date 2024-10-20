using System;
using Windows.UI.Xaml.Controls;

namespace Galaga.Model
{
    /// <summary>
    /// Manages the enemies in the game.
    /// </summary>
    public class EnemyManager
    {
        #region Data members

        private readonly Canvas canvas;
        private readonly double canvasHeight;
        private readonly double canvasWidth;

        #endregion

        #region Constructors

        public EnemyManager(Canvas canvas)
        {
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));

            this.canvas = canvas;
            this.canvasHeight = canvas.Height;
            this.canvasWidth = canvas.Width;

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
            
        }

        #endregion
    }
}