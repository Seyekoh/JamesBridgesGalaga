using System;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    ///     Represents an enemy in the game
    /// </summary>
    public class Enemy : GameObject
    {
        #region Data members

        private const int Level1Score = 10;
        private const int Level2Score = 20;
        private const int Level3Score = 30;
        private const int Level4Score = 40;

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        /// <summary>
        ///     Enum type of the enemy
        /// </summary>
        public GlobalEnums.EnemySpriteType type;

        #endregion

        #region Properties

        /// <summary>
        ///     Initial x location of the enemy
        /// </summary>
        public double InitialX { get; set; }

        /// <summary>
        ///     Boolean to determine if the enemy is moving right
        /// </summary>
        public bool MovingRight { get; set; } = true;

        /// <summary>
        ///     Score given to player when ship is destroyed
        /// </summary>
        public int Score { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Enemy" /> class.
        ///     Default type 1
        /// </summary>
        public Enemy()
        {
            Sprite = new EnemySprite_type1();
            this.type = GlobalEnums.EnemySpriteType.TYPE1;
            SetSpeed(SpeedXDirection, SpeedYDirection);
            this.Score = Level1Score;
        }

        /// <summary>
        ///     Initialized a new instance of the <see cref="Enemy" /> class
        ///     based on the provided type.
        /// </summary>
        /// <param name="enemyType">
        ///     The type of enemy ship to create
        /// </param>
        /// <exception cref="ArgumentException"></exception>
        public Enemy(GlobalEnums.EnemySpriteType enemyType)
        {
            if (enemyType.Equals(GlobalEnums.EnemySpriteType.TYPE1))
            {
                Sprite = new EnemySprite_type1();
                this.type = GlobalEnums.EnemySpriteType.TYPE1;
                this.Score = Level1Score;
            }
            else if (enemyType.Equals(GlobalEnums.EnemySpriteType.TYPE2))
            {
                Sprite = new EnemySprite_type2();
                this.type = GlobalEnums.EnemySpriteType.TYPE2;
                this.Score = Level2Score;
            }
            else if (enemyType.Equals(GlobalEnums.EnemySpriteType.TYPE3))
            {
                Sprite = new EnemySprite_type3();
                this.type = GlobalEnums.EnemySpriteType.TYPE3;
                this.Score = Level3Score;
            }
            else if (enemyType.Equals(GlobalEnums.EnemySpriteType.TYPE4))
            {
                Sprite = new EnemySprite_type4();
                this.type = GlobalEnums.EnemySpriteType.TYPE4;
                this.Score = Level4Score;
            }
            else
            {
                throw new ArgumentException("Invalid enemy type");
            }

            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Allows the enemy to shoot a bullet
        /// </summary>
        /// <returns>
        ///     Bullet that enemy shoots
        /// </returns>
        public Bullet Shoot()
        {
            var bullet = new Bullet();
            bullet.X = X + Width / 2 - bullet.Width / 2;
            bullet.Y = Y + Height;
            return bullet;
        }

        #endregion
    }
}