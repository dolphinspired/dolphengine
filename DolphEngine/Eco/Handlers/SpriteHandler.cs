using DolphEngine.Eco.Components;
using DolphEngine.Graphics.Directives;

namespace DolphEngine.Eco.Handlers
{
    public class SpriteHandler : EcosystemHandler<SpriteComponent>
    {
        public override void Draw(Entity entity)
        {
            var sprite = entity.GetComponent<SpriteComponent>();

            if (!TryGetAnimatedSprite(sprite, out Rect2d src) && !TryGetStaticSprite(sprite, out src))
            {
                // Can't determine a valid sprite to draw
                return;
            }

            Rect2d dest = entity.Rect;

            // Get the origin before any transformations are applied
            Vector2d origin = (dest.GetOriginPosition() - dest.TopLeft).ToVector();

            Rotation2d rotation = sprite.Rotation;
            if (sprite.RotationAnimation != null)
            {
                rotation.Turn(sprite.RotationAnimation.GetFrame(this.Timer.Total));
            }

            dest.Shift(sprite.Offset.X, sprite.Offset.Y);
            if (sprite.OffsetAnimation != null)
            {
                var animShift = sprite.OffsetAnimation.GetFrame(this.Timer.Total);
                dest.Shift(animShift.X, animShift.Y);
            }

            dest.Scale(sprite.Scale.X, sprite.Scale.Y);
            if (sprite.ScaleAnimation != null)
            {
                var animScale = sprite.ScaleAnimation.GetFrame(this.Timer.Total);
                dest.Scale(animScale.X, animScale.Y);
            }

            entity.SetDirective<SpriteDirective>("simple-sprite", sd => 
            {
                sd.Asset = sprite.SpriteSheet.Name;
                sd.Source = src;
                sd.Destination = dest.GetOriginPosition();
                sd.Size = dest.GetSize();
                sd.Rotation = rotation.Radians;
                sd.Origin = origin;
            });

            if (sprite.EnableBoxOutline)
            {
                entity.SetDirective<PolygonDirective>("simple-sprite-box-outline", pd =>
                {
                    pd.Color = new ColorRGBA(255, 255, 0);
                    pd.Points = dest.ToPolygon().Points;
                });
            }
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
