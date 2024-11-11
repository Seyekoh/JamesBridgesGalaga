// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Galaga.View.Sprites
{
    /// <summary>
    ///     The player sprite.
    /// </summary>
    /// <seealso cref="Galaga.View.Sprites.BaseSprite" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public partial class EnemySprite_type1
    {
        #region Data members

        private bool isEngineColorToggled;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnemySprite_type1" /> class.
        /// </summary>
        public EnemySprite_type1()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Animates the engine color.
        /// </summary>
        public virtual void ToggleEngineColor()
        {
            if (this.LeftEngine != null &&
                this.RightEngine != null)
            {
                var newColor = this.isEngineColorToggled ? Colors.Red : Colors.Maroon;
                var brush = new SolidColorBrush(newColor);

                this.LeftEngine.Stroke = brush;
                this.RightEngine.Stroke = brush;

                this.isEngineColorToggled = !this.isEngineColorToggled;
            }
        }

        #endregion

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.LeftEngine != null &&
                this.RightEngine != null)
            {
                var initialColor = new SolidColorBrush(Colors.Red);
                this.LeftEngine.Stroke = initialColor;
                this.RightEngine.Stroke = initialColor;
            }
        }
    }
}