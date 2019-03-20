namespace DolphEngine.Eco.Components
{
    public class PositionComponent2d : Component
    {
        public PositionComponent2d() : this(0, 0) { }

        public PositionComponent2d(float x, float y)
        {
            this.Set(x, y);
        }

        public float X;

        public float Y;

        public void Set(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
