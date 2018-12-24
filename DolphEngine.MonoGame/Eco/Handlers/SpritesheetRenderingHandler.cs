using DolphEngine.Eco;
using DolphEngine.MonoGame.Eco.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class SpritesheetRenderingHandler : SpriteRenderingHandler
    {
        public override IEnumerable<Type> SubscribesTo => new[] { typeof(SpritesheetComponent) };

        public SpritesheetRenderingHandler(SpriteBatch sb) : base(sb)
        {
        }

        public override void Draw(Entity entity)
        {
            var spritesheet = entity.GetComponent<SpritesheetComponent>();
            base.DrawSprite(entity, spritesheet);
        }
    }
}
