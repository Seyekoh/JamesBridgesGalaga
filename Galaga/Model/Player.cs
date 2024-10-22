using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Represents a player in the game.
    /// </summary>
    public class Player : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        public Player()
        {
            Sprite = new PlayerSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Allows the player to shoot a bullet.
        /// </summary>
        /// <returns>
        ///     The bullet being shot.
        /// </returns>
        public Bullet Shoot()
        {
            var bullet = new Bullet();
            bullet.X = X + Width / 2.0 - bullet.Width / 2;
            bullet.Y = Y;

            return bullet;
        }

        /// <summary>
        ///     Gets the bounding box of the player.
        /// </summary>
        /// <returns>
        ///     Rect of the bounding box of the player.
        /// </returns>
        public Rect GetBoundingBox()
        {
            return new Rect(X, Y, Width, Height);
        }

        #endregion
    }
}