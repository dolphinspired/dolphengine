using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.MonoGame.Eco.Components;
using Microsoft.Xna.Framework;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class SpriteHandler : EcosystemHandler<SpriteComponent, DrawComponent>
    {
        public override void Draw(Entity entity)
        {
            var sprite = entity.GetComponent<SpriteComponent>();
            var draw = entity.GetComponent<DrawComponent>();

            this.AddDrawDelegate(entity, sprite, draw);
        }

        public void AddDrawDelegate(Entity entity, SpriteComponent spriteComponent, DrawComponent drawComponent)
        {
            Rectangle src;
            if (spriteComponent.SourceRect != null)
            {
                // 1ST PRIORITY: If a specific subset of the texture was specified, draw only that portion
                src = spriteComponent.SourceRect.Value;
            }
            else
            {
                // LOWEST PRIORITY: Draw the whole texture
                src = spriteComponent.Texture.Bounds;
            }

            int x;
            int y;
            if (spriteComponent.Position != null)
            {
                // 1ST PRIORITY: If the sprite was given an explicit drawing position, use that
                x = spriteComponent.Position.Value.X;
                y = spriteComponent.Position.Value.Y;
            }
            else if (entity.TryGetComponent<PositionComponent2d>(out var position))
            {
                // 2ND PRIORITY: If the entity has a location, draw the sprite there
                x = position.X;
                y = position.Y;
            }
            else
            {
                // LOWEST PRIORITY: Draw sprite at 0,0
                x = 0;
                y = 0;
            }

            int width;
            int height;
            if (spriteComponent.Size != null)
            {
                // 1ST PRIORITY: If the sprite was given an explicit width or height, draw it to that size
                width = spriteComponent.Size.Value.Width;
                height = spriteComponent.Size.Value.Height;
            }
            else if (entity.TryGetComponent<SizeComponent2d>(out var size))
            {
                // 2ND PRIORITY: If the entity has a size, draw the sprite to match
                width = size.Width;
                height = size.Height;
            }
            else
            {
                // LOWEST PRIORITY: Otherwise, draw it to match the size of the whole texture
                width = src.Width;
                height = src.Height;
            }

            Rectangle dest = new Rectangle(x, y, width, height);

            // Add an action that tells the appropriate handler how to draw this entity with said handler's own SpriteBatch
            drawComponent.DrawDelegates.Add(sb => sb.Draw(spriteComponent.Texture, dest, src, spriteComponent.Color ?? Color.White));
        }
    }
}
