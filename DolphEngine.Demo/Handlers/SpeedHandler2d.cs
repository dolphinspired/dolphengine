using DolphEngine.Eco;
using DolphEngine.Eco.Components;

namespace DolphEngine.Demo.Handlers
{
    public class SpeedHandler2d : EcosystemHandler<SpeedComponent2d>
    {
        public override void Update(Entity entity)
        {
            var speed = entity.GetComponent<SpeedComponent2d>();

            if (entity.TryGetComponent<PositionComponent2d>(out var position))
            {
                position.X += speed.X;
                position.Y += speed.Y;
            }
        }
    }
}
