namespace DolphEngine.Eco.Components
{
    public class PositionComponent2d : Component
    {
        public PositionComponent2d(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X;

        public int Y;
    }
}
