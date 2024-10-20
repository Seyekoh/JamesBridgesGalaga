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

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 0;

        public GlobalEnums.EnemySpriteType type;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class.
        /// Default type 1
        /// </summary>
        public Enemy()
        {
            Sprite = new EnemySprite_type1();
            this.type = GlobalEnums.EnemySpriteType.TYPE1;
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
        public Enemy(GlobalEnums.EnemySpriteType enemyType)
        {
            if (enemyType.Equals(GlobalEnums.EnemySpriteType.TYPE1))
            {
                Sprite = new EnemySprite_type1();
                this.type = GlobalEnums.EnemySpriteType.TYPE1;
            }
            else if (enemyType.Equals(GlobalEnums.EnemySpriteType.TYPE2))
            {
                Sprite = new EnemySprite_type2();
                this.type = GlobalEnums.EnemySpriteType.TYPE2;
            }
            else if (enemyType.Equals(GlobalEnums.EnemySpriteType.TYPE3))
            {
                Sprite = new EnemySprite_type3();
                this.type = GlobalEnums.EnemySpriteType.TYPE3;
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