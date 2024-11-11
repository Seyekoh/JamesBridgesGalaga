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
        private readonly BulletManager bulletManager;
        private readonly EnemyManager enemyManager;

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

            this.bulletManager = new BulletManager(Height);
            this.enemyManager = new EnemyManager(this.canvas, bulletManager);
            this.gameManager = new GameManager(this.canvas, this.bulletManager, this.enemyManager);

            this.gameManager.OnScoreUpdated += this.UpdateScoreDisplay;
            this.gameManager.OnPlayerLivesUpdated += this.UpdatePlayerLivesDisplay;
            this.gameManager.OnGameOver += this.DisplayGameOver;

            this.bulletManager.OnSpriteAdded += this.AddSpriteToCanvas;
            this.bulletManager.OnSpriteRemoved += this.RemoveSpriteFromCanvas;

            //this.enemyManager.OnSpriteAdded += this.AddSpriteToCanvas;
            //this.enemyManager.OnSpriteRemoved += this.RemoveSpriteFromCanvas;
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
                this.gameManager.movePlayerLeft();
            }

            if (this.isMovingRight)
            {
                this.gameManager.movePlayerRight();
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
                    this.gameManager.playerShoot();
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

        private void UpdateScoreDisplay(int newScore)
        {
            this.ScoreText.Text = "Score: " + newScore;
        }

        private void UpdatePlayerLivesDisplay(int newLives)
        {
            this.PlayerLives.Text = "Lives: " + newLives;
        }

        private void DisplayGameOver(GlobalEnums.GameOverType gameOverType)
        {
            string gameOverMessage = string.Empty;

            switch (gameOverType)
            {
                case GlobalEnums.GameOverType.WIN:
                    gameOverMessage = "YOU WIN!";
                    break;
                case GlobalEnums.GameOverType.LOSE:
                    gameOverMessage = "YOU LOSE!";
                    break;
            }
            this.GameOverText.Text = "GAME OVER \n" + gameOverMessage;
        }

        private void AddSpriteToCanvas(UIElement sprite)
        {
            this.canvas.Children.Add(sprite);
        }

        private void RemoveSpriteFromCanvas(UIElement sprite)
        {
            this.canvas.Children.Remove(sprite);
        }

        #endregion
    }
}