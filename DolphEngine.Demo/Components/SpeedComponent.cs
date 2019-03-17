using DolphEngine.Eco;

namespace DolphEngine.Demo.Components
{
    public class SpeedComponent : Component
    {
        public SpeedComponent() : this(0, 0) { }

        public SpeedComponent(int x, int y)
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
