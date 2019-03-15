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
                .AddComponent<SpeedComponent2d>()
                .AddComponent<SpriteComponent>()
                .AddComponent<TextComponent>();

            this.Sprite.SpriteSheet = Sprites.Alphonse;
            this.Sprite.Animation = Animations.Player;
        }

        public PositionComponent2d Position => this.GetComponent<PositionComponent2d>();

        public SpeedComponent2d Speed => this.GetComponent<SpeedComponent2d>();

        public SpriteComponent Sprite => this.GetComponent<SpriteComponent>();

        public TextComponent Text => this.GetComponent<TextComponent>();
    }
}
