using DolphEngine.Eco;
using DolphEngine.MonoGame.Eco.Components;
using System;
using System.Collections.Generic;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class AnimatedSpriteHandler : EcosystemHandler<AnimatedSpriteComponent, DrawComponent>
    {
        private readonly SpriteHandler _spriteRenderingHandler;        
        private readonly Func<long> _timer;

        public AnimatedSpriteHandler(Func<long> timer)
        {
            this._spriteRenderingHandler = new SpriteHandler();
            this._timer = timer;
        }

        public override void Draw(IEnumerable<Entity> entities)
        {
            var currentGameTick = this._timer();

            foreach (var entity in entities)
            {
                var animSprite = entity.GetComponent<AnimatedSpriteComponent>();
                if (animSprite.Sequence == null)
                {
                    // If no animation sequence is specified, don't attempt to draw anything
                    continue;
                }

                // Figure out which step of the animation sequence we're in based on the current time
                long currentAnimationTick = currentGameTick - animSprite.StartingTick;
                if (currentAnimationTick < 0)
                {
                    // If the game time hasn't reached the animation's starting tick yet, do not draw the sprite
                    continue;
                }
                long sequenceIndex = currentAnimationTick / animSprite.DurationPerFrame;

                // Get an adjusted value for when the sequence has been exceeded
                int sequenceIndexAdjusted;
                switch (animSprite.Behavior)
                {
                    case AnimatedSpriteBehavior.Loop:
                        // If you've gone past the last frame, start from frame 0 and count up indefinitely
                        sequenceIndexAdjusted = (int)(sequenceIndex % animSprite.Sequence.Count);
                        break;
                    case AnimatedSpriteBehavior.HoldOnLastFrame:
                        // If you've gone past the last frame, just keep drawing the last frame
                        sequenceIndexAdjusted = Math.Min((int)(currentGameTick / animSprite.DurationPerFrame), animSprite.Sequence.Count - 1);
                        break;
                    case AnimatedSpriteBehavior.DisappearAfterLastFrame:
                        if (sequenceIndex > animSprite.Sequence.Count)
                        {
                            // If you've gone past the last frame, do not draw the sprite
                            continue;
                        }
                        sequenceIndexAdjusted = (int)sequenceIndex;
                        break;
                    default:
                        throw new InvalidOperationException($"Unrecognized {nameof(AnimatedSpriteBehavior)}: {animSprite.Behavior}");
                }

                // Then, lookup which frame is specified at that order in the sequence
                animSprite.CurrentFrame = animSprite.Sequence[sequenceIndexAdjusted];

                var draw = entity.GetComponent<DrawComponent>();

                // Call the method that's used to render static sprites with the adjusted source rectangle applied
                this._spriteRenderingHandler.AddDrawDelegate(entity, animSprite, draw);
            }
        }
    }
}
