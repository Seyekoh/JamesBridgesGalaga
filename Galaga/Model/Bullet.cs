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
            Sprite = new EnemyBulletSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        /// <summary>
        ///     Constructor for a player's bullet
        /// </summary>
        /// <param name="player">
        ///     The player that shot the bullet.
        /// </param>
        public Bullet(Player player)
        {
            Sprite = new PlayerBulletSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

    }
}