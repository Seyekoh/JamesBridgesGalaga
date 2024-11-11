namespace Galaga.Model
{
    /// <summary>
    ///     Class contains enums to be used throughout the game.
    /// </summary>
    public class GlobalEnums
    {
        #region Enums

        /// <summary>
        ///     Enum for the enemy sprite types.
        /// </summary>
        public enum EnemySpriteType
        {
            /// <summary>
            ///     Represents Type 1 enemy sprite.
            /// </summary>
            TYPE1,

            /// <summary>
            ///     Represents Type 2 enemy sprite.
            /// </summary>
            TYPE2,

            /// <summary>
            ///     Represents Type 3 enemy sprite.
            /// </summary>
            TYPE3,

            /// <summary>
            ///    Represents Type 4 enemy sprite.
            /// </summary>
            TYPE4
        }

        /// <summary>
        ///     Enum for the game over types.
        /// </summary>
        public enum GameOverType
        {
            /// <summary>
            ///     Represents a win.
            /// </summary>
            WIN,
            /// <summary>
            ///     Represents a loss.
            /// </summary>
            LOSE
        }

        #endregion
    }
}