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
                entity.Space.MoveTo(lp.GetPosition(lp.Target));
            }
            else
            {
                entity.Space.MoveTo(lp.Target.Space);
            }
        }
    }
}
