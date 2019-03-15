using DolphEngine.Graphics.Sprites;

namespace DolphEngine.Eco.Components
{
    public class SpriteComponent : Component
    {
        public SpriteAnimationLibrary Animation;

        public string AnimationSequence;

        public SpriteSheet SpriteSheet;

        public int SpriteSheetIndex;

        public Transform2d? Transform;
    }
}
