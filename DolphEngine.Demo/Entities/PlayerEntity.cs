using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.MonoGame.Eco.Components;

namespace DolphEngine.Demo.Entities
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity()
        {
            this.AddComponent<PositionComponent2d>()
                .AddComponent<SpeedComponent2d>()
                .AddComponent<DrawComponent>()
                .AddComponent<AnimatedSpriteComponent>();
        }

        public PositionComponent2d Position => this.GetComponent<PositionComponent2d>();

        public SpeedComponent2d Speed => this.GetComponent<SpeedComponent2d>();

        public AnimatedSpriteComponent Animation => this.GetComponent<AnimatedSpriteComponent>();
    }
}
