using DolphEngine.Eco;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.MonoGame.Eco.Components
{
    public class TextComponent : Component
    {
        public string Text { get; set; }

        public SpriteFont SpriteFont { get; set; }

        public Color? Color { get; set; }
    }
}
