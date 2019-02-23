using System.Collections.Generic;

namespace DolphEngine.MonoGame.Eco.Components
{
    public class AnimatedSpriteComponent : AtlasSpriteComponent
    {
        public long StartingTick;

        public long DurationPerFrame;
        
        public List<int> Sequence;

        public AnimatedSpriteBehavior Behavior;
    }

    public enum AnimatedSpriteBehavior
    {
        Loop = 0,

        HoldOnLastFrame = 1,

        DisappearAfterLastFrame = 2
    }
}
