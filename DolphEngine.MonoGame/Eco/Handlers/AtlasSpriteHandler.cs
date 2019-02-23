using DolphEngine.Eco;
using DolphEngine.MonoGame.Eco.Components;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class AtlasSpriteHandler : EcosystemHandler<AtlasSpriteComponent, DrawComponent>
    {
        private readonly SpriteHandler _spriteHandler;

        public AtlasSpriteHandler()
        {
            this._spriteHandler = new SpriteHandler();
        }

        public override void Draw(Entity entity)
        {
            var spritesheet = entity.GetComponent<AtlasSpriteComponent>();
            var draw = entity.GetComponent<DrawComponent>();

            if (spritesheet.Atlas == null)
            {
                // Cannot draw from spritesheet if no atlas is specified
                return;
            }

            this._spriteHandler.AddDrawDelegate(entity, spritesheet, draw);
        }
    }
}
