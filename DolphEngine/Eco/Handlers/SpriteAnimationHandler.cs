using DolphEngine.Eco.Components;
using System;
using System.Collections.Generic;

namespace DolphEngine.Eco.Handlers
{
    public class SpriteAnimationHandler : EcosystemHandler<SpriteAnimationComponent, SpriteAtlasComponent>
    {
        private readonly Func<long> _timer;

        public SpriteAnimationHandler(Func<long> timer)
        {
            this._timer = timer;
        }

        public override void Draw(IEnumerable<Entity> entities)
        {
            var currentGameTick = this._timer();

            foreach (var entity in entities)
            {
                var anim = entity.GetComponent<SpriteAnimationComponent>();
                var atlas = entity.GetComponent<SpriteAtlasComponent>();

                if (anim.Sequence == null || anim.Sequence.Count == 0)
                {
                    // If no animation sequence is specified, don't attempt to draw anything
                    continue;
                }
                if (anim.DurationPerFrame <= 0)
                {
                    // Cannot draw a sprite without zero or lower frame duration
                    continue;
                }

                // Figure out which step of the animation sequence we're in based on the current time
                long currentAnimationMs = (currentGameTick - anim.StartingTick) / TimeSpan.TicksPerMillisecond;
                if (currentAnimationMs < 0)
                {
                    // If the game time hasn't reached the animation's starting tick yet, do not draw the sprite
                    continue;
                }

                long sequenceIndex = currentAnimationMs / anim.DurationPerFrame;

                // Get an adjusted value for when the sequence has been exceeded
                int sequenceIndexAdjusted;
                switch (anim.Behavior)
                {
                    case SpriteAnimationBehavior.Loop:
                        // If you've gone past the last frame, start from frame 0 and count up indefinitely
                        sequenceIndexAdjusted = (int)(sequenceIndex % anim.Sequence.Count);
                        break;
                    case SpriteAnimationBehavior.HoldOnLastFrame:
                        // If you've gone past the last frame, just keep drawing the last frame
                        sequenceIndexAdjusted = Math.Min((int)(currentGameTick / anim.DurationPerFrame), anim.Sequence.Count - 1);
                        break;
                    case SpriteAnimationBehavior.DisappearAfterLastFrame:
                        if (sequenceIndex > anim.Sequence.Count)
                        {
                            // If you've gone past the last frame, do not draw the sprite
                            continue;
                        }
                        sequenceIndexAdjusted = (int)sequenceIndex;
                        break;
                    default:
                        throw new InvalidOperationException($"Unrecognized {nameof(SpriteAnimationBehavior)}: {anim.Behavior}");
                }

                // Then, lookup which frame is specified at that order in the sequence
                atlas.Index = anim.Sequence[sequenceIndexAdjusted];
            }
        }
    }
}
