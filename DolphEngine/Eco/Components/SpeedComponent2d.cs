namespace DolphEngine.Eco.Components
{
    public class SpeedComponent2d : Component
    {
        public SpeedComponent2d() : this(0, 0) { }

        public SpeedComponent2d(int x, int y)
        {
            this.Set(x, y);
        }

        public int X;

        public int Y;

        public void Set(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
