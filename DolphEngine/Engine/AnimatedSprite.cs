using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TacticsGame.Engine
{
    public class AnimatedSprite : ISpritable
    {
        public Point Dest { get; set; }

        public string AssetName { get; private set; }

        private AtlasInfo _atlasInfo;

        private AnimationSchedule _animationSchedule;

        private int _animationFrameIndex = -1;

        private long _nextFrameTimeMs;

        private Texture2D _texture;

        private Point _srcSize;

        public AnimatedSprite(string assetName, AtlasInfo atlasInfo, AnimationSchedule animationSchedule)
        {
            this.AssetName = assetName;
            this._atlasInfo = atlasInfo;
            this._animationSchedule = animationSchedule;
        }

        public void Load(ContentManager contentManager)
        {
            this._texture = contentManager.Load<Texture2D>(this.AssetName);

            this._srcSize = new Point(
                this._texture.Width / this._atlasInfo.NumTilesWide,
                this._texture.Height / this._atlasInfo.NumTilesTall);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds >= this._nextFrameTimeMs)
            {
                this._animationFrameIndex++;

                if (this._animationFrameIndex >= this._animationSchedule.Pattern.Count)
                {
                    this._animationFrameIndex = 0;
                }

                this._nextFrameTimeMs = (int)gameTime.TotalGameTime.TotalMilliseconds + this._animationSchedule.Pattern[this._animationFrameIndex].DurationMs;
            }

            var spriteIndex = this._animationSchedule.Pattern[this._animationFrameIndex].Frame;
            var tileColIndex = (int)(spriteIndex % this._atlasInfo.NumTilesWide);
            var tileRowIndex = (int)(spriteIndex / this._atlasInfo.NumTilesWide);
            var src = new Point(tileColIndex * this._srcSize.X, tileRowIndex * this._srcSize.Y);

            var srcRect = new Rectangle(src, this._srcSize);
            var destRect = new Rectangle(this.Dest, this._srcSize);

            spriteBatch.Draw(this._texture, destRect, srcRect, Color.White);
        }
    }
}