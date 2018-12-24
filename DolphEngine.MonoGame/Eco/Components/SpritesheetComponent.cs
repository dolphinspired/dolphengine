using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.MonoGame.Eco.Components
{
    public class SpritesheetComponent : SpriteComponent
    {
        public override Texture2D Texture
        {
            get => this.Tileset.Texture;
            set => this.Tileset.Texture = value;
        }

        public override Rectangle? SourceRect
        {
            get => this.Tileset.Frames[this.CurrentFrame];
            set => base.SourceRect = value; // This value will ultimately get ignored in favor of CurrentFrame
        }

        public int CurrentFrame;

        public Tileset Tileset;
    }
}
