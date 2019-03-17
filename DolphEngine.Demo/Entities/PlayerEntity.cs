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
                .AddComponent<SpriteComponent>()
                .AddComponent<TextComponent>();

            this.Sprite.SpriteSheet = Sprites.Alphonse;
            this.Sprite.Animation = Animations.Player;
        }

        public PositionComponent2d Position => this.GetComponent<PositionComponent2d>();

        public SpeedComponent Speed => this.GetComponent<SpeedComponent>();

        public SpriteComponent Sprite => this.GetComponent<SpriteComponent>();

        public TextComponent Text => this.GetComponent<TextComponent>();
    }
}
