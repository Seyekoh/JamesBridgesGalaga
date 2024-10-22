namespace Galaga.Model
{
    /// <summary>
    ///     Rectangular shape used for collision detection.
    /// </summary>
    public struct Rect
    {
        #region Properties

        /// <summary>
        ///     Rectangle's x-coordinate.
        /// </summary>
        public double X { get; }

        /// <summary>
        ///     Rectangle's y-coordinate.
        /// </summary>
        public double Y { get; }

        /// <summary>
        ///     Rectangle's width.
        /// </summary>
        public double Width { get; }

        /// <summary>
        ///     Rectangle's height.
        /// </summary>
        public double Height { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructor for the Rect struct.
        /// </summary>
        /// <param name="x">
        ///     x-coordinate of the rectangle.
        /// </param>
        /// <param name="y">
        ///     y-coordinate of the rectangle.
        /// </param>
        /// <param name="width">
        ///     width of the rectangle.
        /// </param>
        /// <param name="height">
        ///     height of the rectangle.
        /// </param>
        public Rect(double x, double y, double width, double height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Checks if the current rectangle intersects with another rectangle.
        /// </summary>
        /// <param name="other">
        ///     Other rectangle to check for intersection with.
        /// </param>
        /// <returns>
        ///     True if the rectangles intersect, false otherwise.
        /// </returns>
        public bool IntersectsWith(Rect other)
        {
            return this.X < other.X + other.Width &&
                   this.X + this.Width > other.X &&
                   this.Y < other.Y + other.Height &&
                   this.Y + this.Height > other.Y;
        }

        /// <summary>
        ///     To string for Rect.
        /// </summary>
        /// <returns>
        ///     Rect as a string.
        /// </returns>
        public override string ToString()
        {
            return $"X: {this.X}, Y: {this.Y}, Width: {this.Width}, Height: {this.Height}";
        }

        #endregion
    }
}