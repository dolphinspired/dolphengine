namespace DolphEngine.Eco.Components
{
    public class CameraComponent : Component
    {
        public CameraComponent()
        {
            this.Zoom = 1.000f;
        }

        public float Zoom;

        public Vector2d Pan;

        public Entity Focus;
    }
}
