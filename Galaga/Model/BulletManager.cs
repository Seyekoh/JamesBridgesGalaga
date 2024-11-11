using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Galaga.Model
{
    /// <summary>
    ///     The bullet manager class.
    /// </summary>
    public class BulletManager
    {
        #region Data Members

        private const int MaxPlayerBullets = 3;
        private readonly Canvas canvas;

        #endregion

        #region Properties
        /// <summary>
        ///     List of player bullets.
        /// </summary>
        public IList<Bullet> PlayerBullets { get; private set; }

        /// <summary>
        ///     List of enemy bullets.
        /// </summary>
        public IList<Bullet> EnemyBullets { get; private set; }
    
        #endregion

        #region Constructors

        /// <summary>
        ///     Bullet manager constructor.
        /// </summary>
        /// <param name="canvas">
        ///     The canvas of the game.
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public BulletManager(Canvas canvas)
        {
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));

            this.canvas = canvas;
            this.EnemyBullets = new List<Bullet>();
            this.PlayerBullets = new List<Bullet>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Updates the bullets in the game.
        /// </summary>
        public void UpdateBullets()
        {
            this.moveEnemyBullets();
            this.movePlayerBullets();
            this.removeOffScreenBullets();
        }

        private void moveEnemyBullets()
        {
            foreach (var bullet in this.EnemyBullets)
            {
                bullet.MoveDown();
            }
        }

        private void movePlayerBullets()
        {
            foreach (var bullet in this.PlayerBullets)
            {
                bullet.MoveUp();
            }
        }

        private void removeOffScreenBullets()
        {
            var bulletsToRemove = new List<Bullet>();

            foreach (var bullet in this.PlayerBullets)
            {
                if (bullet.Y < 0)
                {
                    bulletsToRemove.Add(bullet);
                }
            }
            foreach (var bullet in bulletsToRemove)
            {
                this.PlayerBullets.Remove(bullet);
                this.canvas.Children.Remove(bullet.Sprite);
            }

            foreach (var bullet in this.EnemyBullets)
            {
                if (bullet.Y > this.canvas.Height)
                {
                    bulletsToRemove.Add(bullet);
                }
            }
            foreach (var bullet in bulletsToRemove)
            {
                this.EnemyBullets.Remove(bullet);
                this.canvas.Children.Remove(bullet.Sprite);
            }
        }

        /// <summary>
        ///     Adds a player bullet to the game.
        /// </summary>
        /// <param name="bullet">
        ///     The bullet to add.
        /// </param>
        public void AddPlayerBullet(Bullet bullet)
        {
            if (this.PlayerBullets.Count < MaxPlayerBullets)
            {
                this.PlayerBullets.Add(bullet);
                this.canvas.Children.Add(bullet.Sprite);
            }
        }

        /// <summary>
        ///     Removes a player bullet from the game.
        /// </summary>
        /// <param name="bullet">
        ///     The bullet to remove.
        /// </param>
        public void RemovePlayerBullet(Bullet bullet)
        {
            this.PlayerBullets.Remove(bullet);
            this.canvas.Children.Remove(bullet.Sprite);
        }

        /// <summary>
        ///     Adds an enemy bullet to the game.
        /// </summary>
        /// <param name="bullet">
        ///     The bullet to add.
        /// </param>
        public void AddEnemyBullet(Bullet bullet)
        {
            this.EnemyBullets.Add(bullet);
            this.canvas.Children.Add(bullet.Sprite);
        }

        /// <summary>
        ///     Removes an enemy bullet from the game.
        /// </summary>
        /// <param name="bullet">
        ///     The bullet to remove.
        /// </param>
        public void RemoveEnemyBullet(Bullet bullet)
        {
            var bulletToRemove = new List<Bullet>();
            bulletToRemove.Add(bullet);

            foreach (var bulletBeingRemoved in bulletToRemove)
            {
                this.EnemyBullets.Remove(bulletBeingRemoved);
                this.canvas.Children.Remove(bulletBeingRemoved.Sprite);
            }
        }

        /// <summary>
        ///     Checks if a bullet has collided with the player.
        /// </summary>
        /// <param name="enemyBullet">
        ///     The enemy bullet.
        /// </param>
        /// <param name="player">
        ///     The player.
        /// </param>
        /// <returns>
        ///     True if the bullet has collided with the player, false otherwise.
        /// </returns>
        public bool IsCollisionWithPlayer(Bullet enemyBullet, Player player)
        {
            var bulletBox = enemyBullet.GetBoundingBox();
            var playerBox = player.GetBoundingBox();

            return bulletBox.IntersectsWith(playerBox);
        }

        /// <summary>
        ///     Checks if a bullet has collided with an enemy.
        /// </summary>
        /// <param name="playerBullet">
        ///     The player's bullet.
        /// </param>
        /// <param name="enemy">
        ///     The enemy to check collision with.
        /// </param>
        /// <returns>
        ///     True if the bullet has collided with an enemy, false otherwise.
        /// </returns>
        public bool IsCollisionWithEnemy(Bullet playerBullet, Enemy enemy)
        {
            var bulletBox = playerBullet.GetBoundingBox();
            var enemyBox = enemy.GetBoundingBox();

            return bulletBox.IntersectsWith(enemyBox);
        }


        #endregion
    }
}