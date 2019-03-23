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
        
        public Rect2d GetFrameRect(TimeSpan time)
        {
            if (this.SpriteSheet == null)
            {
                throw new InvalidOperationException($"{nameof(SpritesheetAnimation)}: No spritesheet has been specified!");
            }

            var frameIndex = base.GetFrame(time);

            if (frameIndex < 0 || frameIndex >= this.SpriteSheet.Frames.Count)
            {
                throw new InvalidOperationException($"{nameof(SpritesheetAnimation)}: Spritesheet '{this.SpriteSheet.Name}' does not contain a frame at index {frameIndex}!");
            }

            return this.SpriteSheet.Frames[frameIndex];
        }
    }
}
