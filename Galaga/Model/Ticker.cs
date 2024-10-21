using System;
using Windows.UI.Xaml;

namespace Galaga.Model
{
    /// <summary>
    /// Represents a ticker in the game.
    /// </summary>
    public class Ticker
    {
        #region Data members

        private DispatcherTimer timer;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new instance of the <see cref="Ticker"/> class.
        /// </summary>
        public Ticker()
        {
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(500);
            this.timer.Tick += this.timer_Tick;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the ticker.
        /// </summary>
        public void Start()
        {
            this.timer.Start();
        }

        /// <summary>
        /// Stops the ticker.
        /// </summary>
        public void Stop()
        {
            this.timer.Stop();
        }

        public event EventHandler Tick;

        private void timer_Tick(object sender, object e)
        {
            this.Tick?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}