using DolphEngine.Demo.Components;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;

namespace DolphEngine.Demo.Entities
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity() : base("Player")
        {
            this.AddComponent<DrawComponent>()
                .AddComponent<PositionComponent2d>()
                .AddComponent<SpeedComponent>()
                .AddComponent<FacingComponent>()
                .AddComponent(new SpriteComponent { SpriteSheet = Sprites.Alphonse, Animation = Animations.Player })
                .AddComponent<TextComponent>();
        }

        public PositionComponent2d Position => this.GetComponent<PositionComponent2d>();

        public SpeedComponent Speed => this.GetComponent<SpeedComponent>();

        public FacingComponent Facing => this.GetComponent<FacingComponent>();

        public TextComponent Text => this.GetComponent<TextComponent>();
    }
}
