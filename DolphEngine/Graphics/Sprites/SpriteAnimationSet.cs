using DolphEngine.Graphics.Animations;
using System;
using System.Collections.Generic;

namespace DolphEngine.Graphics.Sprites
{
    public class SpriteAnimationSet
    {
        public SpriteAnimationSet(SpriteSheet spritesheet)
        {
            this.SpriteSheet = spritesheet;
        }

        public readonly SpriteSheet SpriteSheet;
        public IReadOnlyDictionary<string, KeyframeAnimation<int>> Animations => this._animations;
        private readonly Dictionary<string, KeyframeAnimation<int>> _animations = new Dictionary<string, KeyframeAnimation<int>>();

        public SpriteAnimationSet AddAnimation(string name, KeyframeAnimation<int> anim)
        {
            if (this._animations.ContainsKey(name))
            {
                throw new ArgumentException($"An animation with name '{name}' has already been added!");
            }

            this._animations.Add(name, anim);
            return this;
        }

        public Rect2d GetFrame(string name, TimeSpan elapsed)
        {
            if (!this.TryGetFrame(name, elapsed, out var frame))
            {
                throw new ArgumentException($"No animation has been added with name '{name}'!");
            }

            return frame;
        }

        public bool TryGetFrame(string name, TimeSpan elapsed, out Rect2d frame)
        {
            if (!this._animations.TryGetValue(name, out var anim))
            {
                frame = Rect2d.Zero;
                return false;
            }

            var frameIndex = anim.GetFrame(elapsed);

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
