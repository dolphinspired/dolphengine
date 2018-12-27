﻿using DolphEngine.Eco;
using DolphEngine.MonoGame.Eco.Components;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class SpritesheetHandler : EcosystemHandler<SpritesheetComponent, DrawComponent>
    {
        private readonly SpriteHandler _spriteRenderingHandler;

        public SpritesheetHandler()
        {
            this._spriteRenderingHandler = new SpriteHandler();
        }

        public override void Draw(Entity entity)
        {
            var spritesheet = entity.GetComponent<SpritesheetComponent>();
            var draw = entity.GetComponent<DrawComponent>();

            this._spriteRenderingHandler.AddDrawDelegate(entity, spritesheet, draw);
        }
    }
}