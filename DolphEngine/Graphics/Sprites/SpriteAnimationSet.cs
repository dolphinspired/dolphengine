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

        public IReadOnlyDictionary<string, KeyframeAnimation<Transform2d>> Transforms => this._transforms;
        private readonly Dictionary<string, KeyframeAnimation<Transform2d>> _transforms = new Dictionary<string, KeyframeAnimation<Transform2d>>();

        public SpriteAnimationSet AddAnimation(string name, KeyframeAnimation<int> anim)
        {
            if (this._animations.ContainsKey(name))
            {
                throw new ArgumentException($"An animation with name '{name}' has already been added!");
            }

            this._animations.Add(name, anim);
            return this;
        }

        public SpriteAnimationSet AddTransform(string name, KeyframeAnimation<Transform2d> transform)
        {
            if (this._transforms.ContainsKey(name))
            {
                throw new ArgumentException($"A transform with name '{name}' has already been added!");
            }

            this._transforms.Add(name, transform);
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

        public Transform2d GetTransform(string name, TimeSpan elapsed)
        {
            if (!this.TryGetTransform(name, elapsed, out var transform))
            {
                throw new ArgumentException($"No transform has been added with name '{name}'!");
            }

            return transform;
        }

        public bool TryGetTransform(string name, TimeSpan elapsed, out Transform2d transform)
        {
            if (!this._transforms.TryGetValue(name, out var tranim))
            {
                transform = Transform2d.None;
                return false;
            }

            transform = tranim.GetFrame(elapsed);
            return true;
        }
    }
}
