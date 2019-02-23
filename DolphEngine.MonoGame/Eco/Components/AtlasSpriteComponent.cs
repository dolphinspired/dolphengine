using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.MonoGame.Eco.Components
{
    public class AtlasSpriteComponent : SpriteComponent
    {
        public override Texture2D Texture
        {
            get => this.Atlas.Texture;
            set => this.Atlas.Texture = value;
        }

        public override Rectangle? SourceRect
        {
            get => this.Atlas.Frames[this.CurrentFrame];
            set => base.SourceRect = value; // This value will ultimately get ignored in favor of CurrentFrame
        }

        public int CurrentFrame;

        public TileAtlas Atlas;
    }
}
