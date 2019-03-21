namespace DolphEngine
{
    public struct Transform2d
    {
        public Transform2d(float offsetX, float offsetY, float scaleX, float scaleY, float rotation)
        {
            this.Offset = new Vector2d(offsetX, offsetY);
            this.Scale = new Vector2d(scaleX, scaleY);
            this.Rotation = rotation;
        }

        public static Transform2d None = new Transform2d(0, 0, 1.000f, 1.000f, 0);

        public Vector2d Offset;

        public Vector2d Scale;

        public float Rotation;

        public override string ToString()
        {
            return $"{{ offset: {Offset}, scale: {Scale}, rotation: {Rotation} }}";
        }
    }
}
