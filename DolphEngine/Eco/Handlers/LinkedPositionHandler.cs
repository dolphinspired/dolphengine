using DolphEngine.Eco.Components;

namespace DolphEngine.Eco.Handlers
{
    public class LinkedPositionHandler : EcosystemHandler<LinkedPositionComponent2d, PositionComponent2d>
    {
        public override void Update(Entity entity)
        {
            var lp = entity.GetComponent<LinkedPositionComponent2d>();
            if (lp.Target == null)
            {
                return;
            }

            if (lp.Target.TryGetComponent<PositionComponent2d>(out var targetPos))
            {
                var pos = entity.GetComponent<PositionComponent2d>();
                pos.X = targetPos.X;
                pos.Y = targetPos.Y;
            }
        }
    }
}
