using DolphEngine.Demo.Components;
using DolphEngine.Eco;

namespace DolphEngine.Demo.Games.TestMap.Handlers
{
    public class SpeedHandler : EcosystemHandler<SpeedComponent>
    {
        public override void Update(Entity entity)
        {
            var speed = entity.GetComponent<SpeedComponent>();

            entity.Space.X += speed.X;
            entity.Space.Y += speed.Y;
        }
    }
}
