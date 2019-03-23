using DolphEngine.Demo.Components;
using DolphEngine.Eco;

namespace DolphEngine.Demo.Handlers
{
    public class SpeedHandler : EcosystemHandler<SpeedComponent>
    {
        public override void Update(Entity entity)
        {
            var speed = entity.GetComponent<SpeedComponent>();

            entity.Space.Position.X += speed.X;
            entity.Space.Position.Y += speed.Y;
        }
    }
}
