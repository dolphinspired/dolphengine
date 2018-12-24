using DolphEngine.Eco;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.MonoGame.Eco.Components
{
    public class SpriteComponent : Component
    {
        public virtual Texture2D Texture { get; set; }

        public virtual Rectangle? SourceRect { get; set; }

        public Position2d? Position;

        public Size2d? Size;

        public Color? Color;
    }
}
