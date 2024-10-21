namespace Galaga.Model
{
    public struct Rect
    {
        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }

        public Rect(double x, double y, double width, double height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool IntersectsWith(Rect other)
        {
            return this.X < other.X + other.Width &&
                   this.X + this.Width > other.X &&
                   this.Y < other.Y + other.Height &&
                   this.Y + this.Height > other.Y;
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Width: {Width}, Height: {Height}";
        }
    }
}