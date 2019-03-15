using System;
using System.Collections.Generic;

namespace DolphEngine.Graphics.Sprites
{
    public class SpriteAnimationLibrary
    {
        public SpriteAnimationLibrary(params SpriteAnimation[] animations)
        {
            this._animations = new Dictionary<string, SpriteAnimation>(animations?.Length ?? 0);

            foreach (var animation in animations)
            {
                this.AddAnimation(animation);
            }
        }
        
        private readonly Dictionary<string, SpriteAnimation> _animations;

        public SpriteAnimationLibrary AddAnimation(SpriteAnimation animation)
        {
            if (this._animations.ContainsKey(animation.Name))
            {
                throw new ArgumentException($"A {nameof(SpriteAnimation)} with name '{animation.Name}' has already been added!");
            }
            this._animations.Add(animation.Name, animation);
            return this;
        }

        public SpriteAnimationLibrary AddAnimation(string name, SpriteSheet spritesheet, params int[] frames)
        {
            return this.AddAnimation(new SpriteAnimation(name, spritesheet, frames));
        }

        public SpriteAnimation GetAnimation(string name)
        {
            if (!this._animations.TryGetValue(name, out var sequence))
            {
                throw new ArgumentException($"No {nameof(SpriteAnimation)} exists with name '{sequence.Name}'!");
            }

            return sequence;
        }

        public bool TryGetAnimation(string name, out SpriteAnimation anim)
        {
            if (name == null)
            {
                anim = null;
                return false;
            }

            return this._animations.TryGetValue(name, out anim);
        }
    }
}
