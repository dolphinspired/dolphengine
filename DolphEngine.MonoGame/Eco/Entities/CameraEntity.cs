using DolphEngine.Eco;
using DolphEngine.Eco.Components;

namespace DolphEngine.MonoGame.Eco.Entities
{
    public class CameraEntity : Entity
    {
        public const float DefaultZoom = 1.000f;

        public CameraEntity(int width, int height) : this(width, height, 0, 0)
        {
        }

        public CameraEntity(int width, int height, int x, int y)
        {
            this.AddComponent(new PositionComponent2d(x, y));
            this.AddComponent(new SizeComponent2d(width, height));
            this.AddComponent(new TransformComponent2d());
            this.AddComponent<SingleTargetComponent>();
        }

        /// <summary>
        /// Represents the top-left corner of the camera in the game window.
        /// </summary>
        public PositionComponent2d Position => this.GetComponent<PositionComponent2d>();

        /// <summary>
        /// Represents the size of the camera's viewport.
        /// </summary>
        public SizeComponent2d Size => this.GetComponent<SizeComponent2d>();

        /// <summary>
        /// Represents how entities will be drawn within the camera's viewport.
        /// </summary>
        public TransformComponent2d Transform => this.GetComponent<TransformComponent2d>();

        /// <summary>
        /// The entity to center the camera upon. If null, the camera will be focused relative to the game window.
        /// </summary>
        public SingleTargetComponent Focus => this.GetComponent<SingleTargetComponent>();

        public CameraEntity AdjustZoom(float zoom)
        {
            var transform = this.Transform;
            transform.Scale.X += zoom;
            transform.Scale.Y += zoom;
            return this;
        }

        public CameraEntity SetZoom(float zoom)
        {
            var transform = this.Transform;
            transform.Scale.X = zoom;
            transform.Scale.Y = zoom;
            return this;
        }

        public CameraEntity ResetZoom()
        {
            var transform = this.Transform;
            transform.Scale.X = 1.000f;
            transform.Scale.Y = 1.000f;
            return this;
        }

        public CameraEntity Pan(int x, int y)
        {
            var transform = this.Transform;
            transform.Offset.X += x;
            transform.Offset.Y += y;
            return this;
        }

        public CameraEntity ResetPan()
        {
            var transform = this.Transform;
            transform.Offset.X = 0;
            transform.Offset.Y = 0;
            return this;
        }
    }
}
