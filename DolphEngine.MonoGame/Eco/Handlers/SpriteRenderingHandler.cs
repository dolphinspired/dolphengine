using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.MonoGame.Eco.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class SpriteRenderingHandler : IEcosystemHandler
    {
        public IEnumerable<Type> SubscribesTo => new List<Type> { typeof(SizeComponent2d), typeof(SpriteComponent2d) };

        private readonly SpriteBatch SpriteBatch;

        public SpriteRenderingHandler(SpriteBatch sb)
        {
            this.SpriteBatch = sb;
        }

        public void Handle(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var positionComponent = entity.GetComponentOrDefault<PositionComponent2d>();
                var sizeComponent = entity.GetComponent<SizeComponent2d>();
                var spriteComponent = entity.GetComponent<SpriteComponent2d>();

                var dest = new Rectangle(positionComponent.X, positionComponent.Y, sizeComponent.Width, sizeComponent.Height);
                this.SpriteBatch.Draw(spriteComponent.Texture, dest, Color.White);
            }
        }
    }
}
