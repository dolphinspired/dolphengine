using System.Collections.Generic;

namespace DolphEngine.Eco.Components
{
    public class SpriteAnimationComponent : Component
    {
        public long StartingTick;

        public long DurationPerFrame;

        public List<int> Sequence;

        public SpriteAnimationBehavior Behavior;
    }

    public enum SpriteAnimationBehavior
    {
        Loop = 0,

        HoldOnLastFrame = 1,

        DisappearAfterLastFrame = 2
    }
}
