using DolphEngine.Graphics.Sprites;

namespace DolphEngine.Eco.Components
{
    public class SpriteComponent : Component
    {
        public SpriteAnimationSet AnimationSet;

        public string AnimatedSprite;

        public string AnimatedTransform;

        public SpriteSheet SpriteSheet;

        public int StaticSprite;

        public Transform2d? StaticTransform;
    }
}
