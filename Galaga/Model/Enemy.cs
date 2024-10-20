using System;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    ///<summary>
    /// Represents an enemy in the game
    /// </summary>
    public class Enemy : GameObject
    {
        #region Data members

        private const String ENEMYTYPE1 = "enemySprite_type1";
        private const String ENEMYTYPE2 = "enemySprite_type2";
        private const String ENEMYTYPE3 = "enemySprite_type3";
        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        public String type;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class.
        /// Default type 1
        /// </summary>
        public Enemy()
        {
            Sprite = new EnemySprite_type1();
            this.type = ENEMYTYPE1;
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        /// <summary>
        /// Initialized a new instance of the <see cref="Enemy"/> class
        /// based on the provided type.
        /// </summary>
        /// <param name="enemyType">
        ///     The type of enemy ship to create
        /// </param>
        /// <exception cref="ArgumentException"></exception>
        public Enemy(String enemyType)
        {
            if (enemyType.Equals(ENEMYTYPE1))
            {
                Sprite = new EnemySprite_type1();
                this.type = ENEMYTYPE1;
            }
            else if (enemyType.Equals(ENEMYTYPE2))
            {
                Sprite = new EnemySprite_type2();
                this.type = ENEMYTYPE2;
            }
            else if (enemyType.Equals(ENEMYTYPE3))
            {
                Sprite = new EnemySprite_type3();
                this.type = ENEMYTYPE3;
            }
            else
            {
                throw new ArgumentException("Invalid enemy type");
            }

            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion
    }
}