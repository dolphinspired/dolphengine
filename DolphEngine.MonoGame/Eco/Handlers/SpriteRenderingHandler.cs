using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.MonoGame.Eco.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class SpriteRenderingHandler : EcosystemHandler<SpriteComponent>
    {
        private readonly SpriteBatch SpriteBatch;

        public SpriteRenderingHandler(SpriteBatch sb)
        {
            this.SpriteBatch = sb;
        }

        public override void Draw(Entity entity)
        {
            var spriteComponent = entity.GetComponent<SpriteComponent>();
            var tRect = spriteComponent.Texture.Bounds;

            // If the entity doesn't have a size, it will be the size of its whole texture
            var sizeComponent = entity.GetComponentOrDefault(new SizeComponent2d(tRect.Width, tRect.Height));

            // If the entity doesn't have a position, it will be drawn at the origin
            var positionComponent = entity.GetComponentOrDefault(new PositionComponent2d(0, 0));

            // If a source rect for the texture is not specified, use the whole texture
            Rectangle src = spriteComponent.SourceRect ?? tRect;
            Rectangle dest = new Rectangle(positionComponent.X, positionComponent.Y, sizeComponent.Width, sizeComponent.Height);

            this.SpriteBatch.Draw(spriteComponent.Texture, dest, src, spriteComponent.Color ?? Color.White);
        }
    }
}
