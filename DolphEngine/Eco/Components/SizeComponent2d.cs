namespace DolphEngine.Eco.Components
{
    public class SizeComponent2d : Component
    {
        public SizeComponent2d(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int Width;

        public int Height;
    }
}
