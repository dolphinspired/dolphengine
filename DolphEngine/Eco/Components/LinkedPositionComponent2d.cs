namespace DolphEngine.Eco.Components
{
    public class LinkedPositionComponent2d : Component
    {
        public LinkedPositionComponent2d(Entity target)
        {
            this.Target = target;
        }

        public Entity Target;
    }
}
