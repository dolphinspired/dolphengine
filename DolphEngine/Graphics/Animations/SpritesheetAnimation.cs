using DolphEngine.Graphics.Sprites;
using System;
using System.Collections.Generic;

namespace DolphEngine.Graphics.Animations
{
    public class SpritesheetAnimation : KeyframeAnimation<int>
    {
        public SpriteSheet SpriteSheet;

        public SpritesheetAnimation(SpriteSheet spritesheet)
        {
            this.SpriteSheet = spritesheet;
        }

        public SpritesheetAnimation(SpriteSheet spritesheet, TimeSpan interval, bool loop, IEnumerable<int> keyframes)
            : base(interval, loop, keyframes)
        {
            this.SpriteSheet = spritesheet;
        }
        
        public bool TryGetFrameRect(TimeSpan time, out Rect2d frame)
        {
            if (this.SpriteSheet == null)
            {
                frame = Rect2d.Zero;
                return false;
            }

            var frameIndex = base.GetFrame(time);

            if (frameIndex < 0 || frameIndex >= this.SpriteSheet.Frames.Count)
            {
                frame = Rect2d.Zero;
                return false;
            }

            frame = this.SpriteSheet.Frames[frameIndex];
            return true;
        }
    }
}
