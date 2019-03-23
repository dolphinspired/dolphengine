using DolphEngine.Eco.Components;
using DolphEngine.Graphics.Directives;

namespace DolphEngine.Eco.Handlers
{
    public class SpriteHandler : EcosystemHandler<SpriteComponent, DrawComponent>
    {
        public override void Draw(Entity entity)
        {
            var sprite = entity.GetComponent<SpriteComponent>();
            var draw = entity.GetComponent<DrawComponent>();

            if (!TryGetAnimatedSprite(sprite, out Rect2d src) && !TryGetStaticSprite(sprite, out src))
            {
                // Can't determine a valid sprite to draw
                return;
            }

            Rect2d dest = new Rect2d();
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

            if (entity.TryGetComponent<PositionComponent2d>(out var position))
            {
                // If the entity has a location, draw the sprite there
                dest.SetPosition(position.X, position.Y);
            }
            else
            {
                // Otherwise, draw at 0,0 centered on the sprite's anchor point
                dest.SetPosition(0, 0);
            }

            Rotation2d rotation = sprite.Rotation;
            if (sprite.RotationAnimation != null)
            {
                rotation.Turn(sprite.RotationAnimation.GetFrame(this.Timer.Total));
            }

            dest.Shift(sprite.Offset);
            if (sprite.OffsetAnimation != null)
            {
                dest.Shift(sprite.OffsetAnimation.GetFrame(this.Timer.Total));
            }

            dest.Scale(sprite.Scale);
            if (sprite.ScaleAnimation != null)
            {
                var animScale = sprite.ScaleAnimation.GetFrame(this.Timer.Total);
                dest.Scale(animScale);
            }

            Vector2d origin = (src.GetPosition(sprite.Origin) - src.GetPosition(Anchor2d.TopLeft)).ToVector();

            var directive = new SpriteDirective
            {
                Asset = sprite.SpriteSheet.Name,
                Source = src,
                Destination = dest,
                Rotation = rotation.Radians,
                Origin = origin
            };

            draw.Directives.Add(directive);
        }

        private bool TryGetAnimatedSprite(SpriteComponent sprite, out Rect2d src)
        {
            if (sprite.SpriteAnimation == null)
            {
                src = Rect2d.Zero;
                return false;
            }

            return sprite.SpriteAnimation.TryGetFrameRect(this.Timer.Total, out src);
        }

        private static bool TryGetStaticSprite(SpriteComponent sprite, out Rect2d src)
        {
            if (sprite.SpriteSheet == null || sprite.Index < 0 || sprite.Index >= sprite.SpriteSheet.Frames.Count)
            {
                // If there is no spritesheet to reference, or an invalid frame is selected, nothing can be drawn
                src = Rect2d.Zero;
                return false;
            }

            src = sprite.SpriteSheet.Frames[sprite.Index];
            return true;
        }
    }
}
