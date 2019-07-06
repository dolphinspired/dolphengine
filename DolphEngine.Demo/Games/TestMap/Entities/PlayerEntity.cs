using DolphEngine.Demo.Components;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;

namespace DolphEngine.Demo.Games.TestMap.Entities
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity()
        {
            this.AddComponent<SpeedComponent>()
                .AddComponent<FacingComponent>()
                .AddComponent(new SpriteComponent { SpriteSheet = Sprites.Alphonse })
                .AddComponent<TextComponent>();

            this.Rect = new Rect2d(0, 0, 32, 64, new Origin2d(Anchor2d.BottomCenter));
        }

        public SpeedComponent Speed => this.GetComponent<SpeedComponent>();

        public FacingComponent Facing => this.GetComponent<FacingComponent>();

        public TextComponent Text => this.GetComponent<TextComponent>();

        public SpriteComponent Sprite => this.GetComponent<SpriteComponent>();
    }
}
