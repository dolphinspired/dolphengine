using System;

namespace DolphEngine.Eco.Components
{
    public class LinkedPositionComponent : Component
    {
        public LinkedPositionComponent(Entity target)
        {
            this.Target = target;
        }

        public LinkedPositionComponent(Entity target, Func<Entity, Position2d> getPosition)
        {
            this.Target = target;
            this.GetPosition = getPosition;
        }

        public Entity Target;

        public Func<Entity, Position2d> GetPosition;
    }
}
