using DolphEngine.Eco.Components;

namespace DolphEngine.Eco.Handlers
{
    public class SpriteAtlasHandler : EcosystemHandler<SpriteAtlasComponent, SpriteComponent>
    {
        public override void Draw(Entity entity)
        {
            var ac = entity.GetComponent<SpriteAtlasComponent>();
            var sc = entity.GetComponent<SpriteComponent>();

            try
            {
                sc.SourceRect = ac.Frames[ac.Index];
            }
            catch (System.IndexOutOfRangeException)
            {
                // An invalid frame was specified, make no changes
            }
        }
    }
}
