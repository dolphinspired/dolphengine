using DolphEngine.Eco;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.MonoGame.Eco.Components
{
    public class SpriteComponent : Component
    {
        public Texture2D Texture;

        public Rectangle? SourceRect;

        public Position2d? Position;

        public Size2d? Size;

        public Color? Color;
    }
}
