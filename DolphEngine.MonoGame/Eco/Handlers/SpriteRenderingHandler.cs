using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.MonoGame.Eco.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class SpriteRenderingHandler : EcosystemHandler<SpriteComponent>
    {
        protected readonly SpriteBatch SpriteBatch;

        public SpriteRenderingHandler(SpriteBatch sb)
        {
            this.SpriteBatch = sb;
        }

        public override void Draw(Entity entity)
        {
            var sprite = entity.GetComponent<SpriteComponent>();
            this.DrawSprite(entity, sprite);
        }

        protected virtual void DrawSprite(Entity entity, SpriteComponent sprite)
        {
            Rectangle src;
            if (sprite.SourceRect != null)
            {
                // 1ST PRIORITY: If a specific subset of the texture was specified, draw only that portion
                src = sprite.SourceRect.Value;
            }
            else
            {
                // LOWEST PRIORITY: Draw the whole texture
                src = sprite.Texture.Bounds;
            }

            int x;
            int y;
            if (sprite.Position != null)
            {
                // 1ST PRIORITY: If the sprite was given an explicit drawing position, use that
                x = sprite.Position.Value.X;
                y = sprite.Position.Value.Y;
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
            if (sprite.Size != null)
            {
                // 1ST PRIORITY: If the sprite was given an explicit width or height, draw it to that size
                width = sprite.Size.Value.Width;
                height = sprite.Size.Value.Height;
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

            this.SpriteBatch.Draw(sprite.Texture, dest, src, sprite.Color ?? Color.White);
        }
    }
}
