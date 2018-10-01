using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.Old
{
    public interface ISpritable
    {
        Point Dest { get; set; }

        string AssetName { get; }

        void Load(ContentManager contentManager);

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}