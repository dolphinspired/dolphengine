using DolphEngine.Eco;
using DolphEngine.Eco.Components;

namespace DolphEngine.Demo
{
    public class GlyphEntity : Entity
    {
        public GlyphEntity(int glyphIndex)
        {
            this.AddComponent(new SpriteComponent { SpriteSheet = Sprites.Glyphs, Index = glyphIndex });
        }

        public SpriteComponent Sprite => this.GetComponent<SpriteComponent>();
    }
}
