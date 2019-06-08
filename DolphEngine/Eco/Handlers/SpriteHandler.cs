﻿using DolphEngine.Eco.Components;
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

            Rect2d dest = entity.Space;

            // Get the origin before any transformations are applied
            Vector2d origin = (dest.Position - dest.TopLeft).ToVector();

            Rotation2d rotation = sprite.Rotation;
            if (sprite.RotationAnimation != null)
            {
                rotation.Turn(sprite.RotationAnimation.GetFrame(this.Timer.Total));
            }

            dest.Position.Shift(sprite.Offset);
            if (sprite.OffsetAnimation != null)
            {
                dest.Position.Shift(sprite.OffsetAnimation.GetFrame(this.Timer.Total));
            }

            dest.Size.Scale(sprite.Scale);
            if (sprite.ScaleAnimation != null)
            {
                var animScale = sprite.ScaleAnimation.GetFrame(this.Timer.Total);
                dest.Size.Scale(animScale);
            }

            var directive = new SpriteDirective
            {
                Asset = sprite.SpriteSheet.Name,
                Source = src,
                Destination = dest.Position,
                Size = dest.Size,
                Rotation = rotation.Radians,
                Origin = origin
            };

            draw.Directives.Add(directive);

            if (sprite.EnableBoxOutline)
            {
                draw.Directives.Add(new PolygonDirective
                {
                    Color = 0xFF00FFFF,
                    Points = dest.ToPolygon().Points
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
