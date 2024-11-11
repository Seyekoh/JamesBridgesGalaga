// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Galaga.View.Sprites
{
    /// <summary>
    ///     The player sprite.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class EnemySprite_type4
    {
        #region Data members

        private bool isEngineColorToggled;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemySprite_type4" /> class.
        /// </summary>
        public EnemySprite_type4()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        ///    Animates the engine color.
        /// </summary>
        public void ToggleEngineColor()
        {
            if (this.Engine != null)
            {
                var newColor = this.isEngineColorToggled ? Windows.UI.Colors.Red : Windows.UI.Colors.Maroon;
                var brush = new Windows.UI.Xaml.Media.SolidColorBrush(newColor);

                this.Engine.Stroke = brush;

                this.isEngineColorToggled = !this.isEngineColorToggled;
            }
        }

        #endregion
    }
}