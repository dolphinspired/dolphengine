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

            if (sprite.TextureAssetName == null || sprite.SourceRect == null)
            {
                // Can't draw without a specified asset or a specified source region
                return;
            }

            Rect2d src = sprite.SourceRect.Value;

            // DEFAULT: Draw sprite at 0,0
            Rect2d dest = new Rect2d();
            if (entity.TryGetComponent<PositionComponent2d>(out var position))
            {
                // If the entity has a location, draw the sprite there
                dest.X = position.X;
                dest.Y = position.Y;
            }
            if (sprite.Transform?.Offset != null)
            {
                // Apply any position transforms
                dest.X += sprite.Transform.Value.Offset.Value.X;
                dest.Y += sprite.Transform.Value.Offset.Value.Y;
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
            if (sprite.Transform?.Scale != null)
            {
                // Apply any size transforms
                dest.Width = (int)(dest.Width * sprite.Transform.Value.Scale.Value.X);
                dest.Height = (int)(dest.Height * sprite.Transform.Value.Scale.Value.Y);
            }

            draw.Directives.Add(new SpriteDirective
            {
                TextureAssetName = sprite.TextureAssetName,
                Source = src,
                Destination = dest,
                Rotation = sprite.Transform?.Rotation ?? 0.000f // Rotation transform will be applied by the drawing engine
            });
        }
    }
}
