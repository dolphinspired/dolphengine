using DolphEngine.Eco.Components;

namespace DolphEngine.Eco.Handlers
{
    public class LinkedPositionHandler : EcosystemHandler<LinkedPositionComponent>
    {
        public override void Update(Entity entity)
        {
            var lp = entity.GetComponent<LinkedPositionComponent>();
            if (lp.Target == null)
            {
                return;
            }

            if (lp.GetPosition != null)
            {
                entity.Space.Position = lp.GetPosition(lp.Target);
            }
            else
            {
                entity.Space.Position.Set(lp.Target.Space.Position);
            }
        }
    }
}
