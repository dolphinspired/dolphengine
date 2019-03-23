using DolphEngine.Eco.Components;

namespace DolphEngine.Eco.Entities
{
    public class CameraEntity : Entity
    {
        public CameraEntity(int width, int height) : this(width, height, 0, 0)
        {
        }

        public CameraEntity(int width, int height, int x, int y) : base("Camera")
        {
            this.Space = new Rect2d(x, y, width, height, Origin2d.TrueCenter);
            this.AddComponent<CameraComponent>();
        }

        /// <summary>
        /// The entity to center the camera upon. If null, the camera will be focused relative to the game window.
        /// </summary>
        public CameraComponent Lens => this.GetComponent<CameraComponent>();
    }
}
