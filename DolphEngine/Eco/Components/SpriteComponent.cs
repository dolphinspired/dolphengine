using DolphEngine.Graphics.Animations;
using DolphEngine.Graphics.Sprites;

namespace DolphEngine.Eco.Components
{
    public class SpriteComponent : Component
    {
        public SpriteComponent()
        {
            this.Scale = Vector2d.One;
        }

        public SpriteSheet SpriteSheet;

        public int Index;

        public SpritesheetAnimation SpriteAnimation;

        public Vector2d Offset;

        public PositionAnimation OffsetAnimation;

        public Rotation2d Rotation;

        public RotationAnimation RotationAnimation;

        public Vector2d Scale;

        public ScaleAnimation ScaleAnimation;

        public bool EnableBoxOutline;
    }
}
