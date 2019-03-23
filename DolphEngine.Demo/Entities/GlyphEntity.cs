using DolphEngine.Eco;
using DolphEngine.Eco.Components;

namespace DolphEngine.Demo.Entities
{
    public class GlyphEntity : Entity
    {
        public GlyphEntity(int glyphIndex, string name) : base(name)
        {
            this.AddComponent(new SpriteComponent { SpriteSheet = Sprites.Glyphs, Index = glyphIndex })
                .AddComponent<DrawComponent>()
                .AddComponent<PositionComponent2d>();
        }

        public PositionComponent2d Position => this.GetComponent<PositionComponent2d>();

        public SpriteComponent Sprite => this.GetComponent<SpriteComponent>();
    }
}
