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

        #region Properties

        /// <summary>
        ///     Property to determine if the player can shoot.
        /// </summary>
        public bool CanShoot { get; set; } = true;
    

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
            if (!this.CanShoot)
            {
                return null;
            }

            var bullet = new Bullet(this);
            bullet.X = X + Width / 2.0 - bullet.Width / 2;
            bullet.Y = Y;

            return bullet;
        }

        #endregion
    }
}