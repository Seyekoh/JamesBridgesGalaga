
using System;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Galaga.View.Sprites;

namespace Galaga.Model
{
    /// <summary>
    /// Represents a bullet in the game.
    /// </summary>
    public class Bullet : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 0;
        private const int SpeedYDirection = 35;

        #endregion

        #region Constructors

        public Bullet()
        {
            Sprite = new BulletSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        public void Move()
        {
            this.Y -= this.SpeedY;
            Canvas.SetTop(this.Sprite, this.Y);
        }

        public Rect getBoundingBox()
        {
            return new Rect(this.X, this.Y, this.Width, this.Height);
        }

        #endregion
    }
}