using System;
using System.Collections.Generic;

namespace DolphEngine.Graphics.Animations
{
    public class AnimationLibrary
    {
        private readonly Dictionary<Type, Dictionary<string, KeyframeAnimation>> _animations = new Dictionary<Type, Dictionary<string, KeyframeAnimation>>();

        public AnimationLibrary AddAnimation<T>(string name, T animation)
            where T : KeyframeAnimation
        {
            if (!this._animations.TryGetValue(typeof(T), out var anims))
            {
                anims = new Dictionary<string, KeyframeAnimation>(1);
                this._animations.Add(typeof(T), anims);
            }

            if (!anims.ContainsKey(name))
            {
                anims.Add(name, animation);
            }
            else
            {
                anims[name] = animation;
            }

            return this;
        }

        public bool TryGetAnimation<T>(string name, out T animation)
            where T : KeyframeAnimation
        {
            if (name == null || !_animations.TryGetValue(typeof(T), out var anims) || !anims.TryGetValue(name, out var anim))
            {
                animation = null;
                return false;
            }

            animation = anim as T;
            return true;
        }

        public T GetAnimation<T>(string name)
            where T : KeyframeAnimation
        {
            if (!this.TryGetAnimation<T>(name, out var anim))
            {
                throw new InvalidOperationException($"{nameof(AnimationLibrary)}: No {typeof(T).Name} exists with name '{name}'!");
            }

            return anim;
        }
    }
}
