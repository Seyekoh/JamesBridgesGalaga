using System;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Galaga.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Galaga.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameCanvas
    {
        #region Data members

        private const double TimerInterval = 16;

        private readonly GameManager gameManager;
        private readonly DispatcherTimer timer = new DispatcherTimer();

        private bool isMovingLeft;
        private bool isMovingRight;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameCanvas" /> class.
        /// </summary>
        public GameCanvas()
        {
            this.InitializeComponent();

            Width = this.canvas.Width;
            Height = this.canvas.Height;
            ApplicationView.PreferredLaunchViewSize = new Size { Width = Width, Height = Height };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(Width, Height));

            this.initializeTimer();

            Window.Current.CoreWindow.KeyDown += this.coreWindowOnKeyDown;
            Window.Current.CoreWindow.KeyUp += this.coreWindowOnKeyUp;

            this.gameManager = new GameManager(this.canvas, this.ScoreText, this.GameOverText, this.PlayerLives);
        }

        #endregion

        #region Methods

        private void initializeTimer()
        {
            this.timer.Interval = TimeSpan.FromMilliseconds(TimerInterval);
            this.timer.Tick += this.GameLoop;
            this.timer.Start();
        }

        private void GameLoop(object sender, object e)
        {
            if (this.isMovingLeft)
            {
                this.gameManager.MovePlayerLeft();
            }

            if (this.isMovingRight)
            {
                this.gameManager.MovePlayerRight();
            }
        }

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.isMovingLeft = true;
                    break;
                case VirtualKey.Right:
                    this.isMovingRight = true;
                    break;
                case VirtualKey.Space:
                    this.gameManager.PlayerShoot();
                    break;
            }
        }

        private void coreWindowOnKeyUp(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.isMovingLeft = false;
                    break;
                case VirtualKey.Right:
                    this.isMovingRight = false;
                    break;
            }
        }

        #endregion
    }
}