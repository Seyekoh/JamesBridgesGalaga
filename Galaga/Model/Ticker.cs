using System;
using Windows.UI.Xaml;

namespace Galaga.Model
{
    /// <summary>
    ///     Represents a ticker in the game.
    /// </summary>
    public class Ticker
    {
        #region Data members

        private const int Interval = 500;

        private readonly DispatcherTimer timer;

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructs a new instance of the <see cref="Ticker" /> class.
        /// </summary>
        public Ticker()
        {
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(Interval);
            this.timer.Tick += this.timer_Tick;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Starts the ticker.
        /// </summary>
        public void Start()
        {
            this.timer.Start();
        }

        /// <summary>
        ///     Stops the ticker.
        /// </summary>
        public void Stop()
        {
            this.timer.Stop();
        }

        /// <summary>
        ///     Event handler for the ticker.
        /// </summary>
        public event EventHandler Tick;

        private void timer_Tick(object sender, object e)
        {
            this.Tick?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}