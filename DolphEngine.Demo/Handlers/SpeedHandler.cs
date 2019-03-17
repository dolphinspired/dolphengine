using DolphEngine.Demo.Components;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;

namespace DolphEngine.Demo.Handlers
{
    public class SpeedHandler : EcosystemHandler<SpeedComponent>
    {
        public override void Update(Entity entity)
        {
            var speed = entity.GetComponent<SpeedComponent>();

            if (entity.TryGetComponent<PositionComponent2d>(out var position))
            {
                position.X += speed.X;
                position.Y += speed.Y;
            }
        }
    }
}
