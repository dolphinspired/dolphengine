using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.MonoGame.Old
{
    public class StaticSprite : ISpritable
    {
        public Point Dest { get; set; }

        public string AssetName { get; private set; }

        private AtlasInfo _atlasInfo;

        private Texture2D _texture;

        private int _spriteIndex;

        private Rectangle _srcRect;

        public StaticSprite(string assetName, AtlasInfo atlasInfo, int spriteIndex)
        {
            this.AssetName = assetName;
            this._atlasInfo = atlasInfo;
            this._spriteIndex = spriteIndex;
        }

        public virtual void Load(ContentManager contentManager)
        {
            this._texture = contentManager.Load<Texture2D>(this.AssetName);
            var tileWidthPx = this._texture.Width / this._atlasInfo.NumTilesWide;
            var tileHeightPx = this._texture.Height / this._atlasInfo.NumTilesTall;
            var tileColIndex = (int)(this._spriteIndex % this._atlasInfo.NumTilesWide);
            var tileRowIndex = (int)(this._spriteIndex / this._atlasInfo.NumTilesWide);

            this._srcRect = new Rectangle(tileColIndex * tileWidthPx, tileRowIndex * tileHeightPx, tileWidthPx, tileHeightPx);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var destRect = new Rectangle(this.Dest.X, this.Dest.Y, this._srcRect.Width, this._srcRect.Height);

            spriteBatch.Draw(this._texture, destRect, this._srcRect, Color.White);
        }
    }
}