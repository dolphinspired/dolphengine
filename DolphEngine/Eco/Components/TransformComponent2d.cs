namespace DolphEngine.Eco.Components
{
    public class TransformComponent2d : Component
    {
        public TransformComponent2d() : this(0, 0, 1.000f, 1.000f, 0.000f)
        {
        }

        public TransformComponent2d(int offsetX, int offsetY, float scaleX, float scaleY, float rotation)
        {
            this.Offset = new Vector2d(offsetX, offsetY);
            this.Scale = new Vector2d(scaleX, scaleY);
            this.Rotation = rotation;
        }

        public Vector2d Offset;

        public Vector2d Scale;

        public float Rotation;
    }
}
