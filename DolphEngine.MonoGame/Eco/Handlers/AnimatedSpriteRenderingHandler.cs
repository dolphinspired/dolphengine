using DolphEngine.Eco;
using DolphEngine.MonoGame.Eco.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class AnimatedSpriteRenderingHandler : SpriteRenderingHandler
    {
        public override IEnumerable<Type> SubscribesTo => new[] { typeof(AnimatedSpriteComponent) };
        
        private readonly Func<long> Timer;

        public AnimatedSpriteRenderingHandler(SpriteBatch sb, Func<long> timer) : base(sb)
        {
            this.Timer = timer;
        }

        public override void Draw(IEnumerable<Entity> entities)
        {
            var currentGameTick = this.Timer();

            foreach (var entity in entities)
            {
                var animSprite = entity.GetComponent<AnimatedSpriteComponent>();

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

                // Call the method that's used to render static sprites with the adjusted source rectangle applied
                this.DrawSprite(entity, animSprite);
            }
        }
    }
}
