using DolphEngine.Eco.Components;

namespace DolphEngine.Eco.Handlers
{
    public class LinkedPositionHandler : EcosystemHandler<LinkedPositionComponent2d>
    {
        public override void Update(Entity entity)
        {
            var lp = entity.GetComponent<LinkedPositionComponent2d>();
            if (lp.Target == null)
            {
                return;
            }

            entity.Space.Position.Set(lp.Target.Space.Position);
        }
    }
}
