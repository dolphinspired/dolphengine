using DolphEngine.Eco.Components;
using DolphEngine.Graphics.Directives;
using System;

namespace DolphEngine.Eco.Handlers
{
    public class SpriteHandler : EcosystemHandler<SpriteComponent, DrawComponent>
    {
        public override void Draw(Entity entity)
        {
            var sprite = entity.GetComponent<SpriteComponent>();
            var draw = entity.GetComponent<DrawComponent>();

            if (sprite.SpriteSheet == null)
            {
                // If there is no spritesheet to reference, nothing can be drawn
                return;
            }

            if (!TryGetAnimationFrame(sprite, out Rect2d src) && !TryGetSpritesheetFrame(sprite, out src))
            {
                // Unable to get a source frame, nothing to draw
                return;
            }

            // DEFAULT: Draw sprite at 0,0
            Rect2d dest = new Rect2d();
            if (entity.TryGetComponent<PositionComponent2d>(out var position))
            {
                // If the entity has a location, draw the sprite there
                dest.X = position.X;
                dest.Y = position.Y;
            }

            if (entity.TryGetComponent<SizeComponent2d>(out var size))
            {
                // If the entity has a size, draw the sprite to match
                dest.Width = size.Width;
                dest.Height = size.Height;
            }
            else
            {
                // Otherwise, simply match the size of the source region
                dest.Width = src.Width;
                dest.Height = src.Height;
            }

            float rotation = 0.000f;
            if (sprite.StaticTransform != null)
            {
                // First, apply any static transforms
                var tf = sprite.StaticTransform.Value;
                dest.X += tf.Offset.X;
                dest.Y += tf.Offset.Y;
                dest.Width *= tf.Scale.X;
                dest.Height *= tf.Scale.Y;
                rotation += tf.Rotation;
            }
            if (TryGetAnimatedTransform(sprite, out var animatedTransform))
            {
                // Then, apply any animated transforms
                var tf = animatedTransform;
                dest.X += tf.Offset.X;
                dest.Y += tf.Offset.Y;
                dest.Width *= tf.Scale.X;
                dest.Height *= tf.Scale.Y;
                rotation += tf.Rotation;
            }

            draw.Directives.Add(new SpriteDirective
            {
                Asset = sprite.SpriteSheet.Name,
                Source = src,
                Destination = dest,
                Rotation = rotation
            });
        }

        private bool TryGetSpritesheetFrame(SpriteComponent sprite, out Rect2d src)
        {
            try
            {
                src = sprite.SpriteSheet.Frames[sprite.StaticSprite];
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                // An invalid frame was specified, nothing to draw
                src = Rect2d.Zero;
                return false;
            }
        }

        private bool TryGetAnimationFrame(SpriteComponent sprite, out Rect2d src)
        {
            if (sprite.AnimationSet == null || sprite.AnimatedSprite == null)
            {
                // No animation info has been added to the component
                src = Rect2d.Zero;
                return false;
            }

            return sprite.AnimationSet.TryGetFrame(sprite.AnimatedSprite, this.Timer.Total, out src);
        }

        private bool TryGetAnimatedTransform(SpriteComponent sprite, out Transform2d transform)
        {
            if (sprite.AnimationSet == null || sprite.AnimatedTransform == null)
            {
                transform = Transform2d.None;
                return false;
            }

            return sprite.AnimationSet.TryGetTransform(sprite.AnimatedTransform, this.Timer.Total, out transform);
        }
    }
}
