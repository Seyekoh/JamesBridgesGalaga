using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Represents a bullet in the game.
    /// </summary>
    public class Bullet : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 0;
        private const int SpeedYDirection = 25;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructor for a bullet
        /// </summary>
        public Bullet()
        {
            Sprite = new BulletSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates a Rect object that represents the bounding box of the bullet.
        /// </summary>
        /// <returns>
        ///     Rect of the bounding box of the bullet.
        /// </returns>
        public Rect GetBoundingBox()
        {
            return new Rect(X, Y, Width, Height);
        }

        #endregion
    }
}