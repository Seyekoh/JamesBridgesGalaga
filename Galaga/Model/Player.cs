using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    public class Player : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        public Player()
        {
            Sprite = new PlayerSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        public Bullet Shoot()
        {
            Bullet bullet = new Bullet();
            bullet.X = this.X + (this.Width / 2.0) - (bullet.Width / 2);
            bullet.Y = this.Y;

            return bullet;
        }

        public Rect GetBoundingBox()
        {
            return new Rect(this.X, this.Y, this.Width, this.Height);
        }

        #endregion
    }
}
