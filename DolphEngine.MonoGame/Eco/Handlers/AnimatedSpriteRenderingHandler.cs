using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.MonoGame.Eco.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class AnimatedSpriteRenderingHandler : EcosystemHandler<AnimatedSpriteComponent>
    {
        private readonly SpriteBatch SpriteBatch;
        private readonly Func<long> Timer;

        public AnimatedSpriteRenderingHandler(SpriteBatch sb, Func<long> timer)
        {
            this.SpriteBatch = sb;
            this.Timer = timer;
        }

        public override void Draw(IEnumerable<Entity> entities)
        {
            var currentGameTick = this.Timer();

            foreach (var entity in entities)
            {
                var spriteComponent = entity.GetComponent<AnimatedSpriteComponent>();
                var tRect = spriteComponent.Texture.Bounds;

                // If the entity doesn't have a size, it will be the size of its whole texture
                var sizeComponent = entity.GetComponentOrDefault(new SizeComponent2d(tRect.Width, tRect.Height));

                // If the entity doesn't have a position, it will be drawn at the origin
                var positionComponent = entity.GetComponentOrDefault(new PositionComponent2d(0, 0));

                // If a source rect for the texture is not specified, use the whole texture
                Rectangle srcWhole = spriteComponent.SourceRect ?? tRect;

                // Figure out which step of the animation sequence we're in based on the current time
                long currentAnimationTick = currentGameTick - spriteComponent.StartingTick;
                if (currentAnimationTick < 0)
                {
                    // If the game time hasn't reached the animation's starting tick yet, do not draw the sprite
                    continue;
                }
                long sequenceIndex = currentAnimationTick / spriteComponent.DurationPerFrame;

                // Get an adjusted value for when the sequence has been exceeded
                int sequenceIndexAdjusted;
                switch (spriteComponent.Behavior)
                {
                    case AnimatedSpriteBehavior.Loop:
                        // If you've gone past the last frame, start from frame 0 and count up indefinitely
                        sequenceIndexAdjusted = (int)(sequenceIndex % spriteComponent.Sequence.Count);
                        break;
                    case AnimatedSpriteBehavior.HoldOnLastFrame:
                        // If you've gone past the last frame, just keep drawing the last frame
                        sequenceIndexAdjusted = Math.Min((int)(currentGameTick / spriteComponent.DurationPerFrame), spriteComponent.Sequence.Count - 1);
                        break;
                    case AnimatedSpriteBehavior.DisappearAfterLastFrame:
                        if (sequenceIndex > spriteComponent.Sequence.Count)
                        {
                            // If you've gone past the last frame, do not draw the sprite
                            continue;
                        }
                        sequenceIndexAdjusted = (int)sequenceIndex;
                        break;
                    default:
                        throw new InvalidOperationException($"Unrecognized {nameof(AnimatedSpriteBehavior)}: {spriteComponent.Behavior}");
                }

                // Then, lookup which frame is specified at that order in the sequence
                int frameIndex = spriteComponent.Sequence[sequenceIndexAdjusted];
                Rectangle src = spriteComponent.Frames[frameIndex];

                Rectangle dest = new Rectangle(positionComponent.X, positionComponent.Y, sizeComponent.Width, sizeComponent.Height);

                this.SpriteBatch.Draw(spriteComponent.Texture, dest, src, spriteComponent.Color ?? Color.White);
            }
        }
    }
}
