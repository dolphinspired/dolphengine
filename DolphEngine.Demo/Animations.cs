using DolphEngine.Graphics.Animations;
using DolphEngine.Graphics.Sprites;
using System;
using System.Collections.Generic;

namespace DolphEngine.Demo
{
    public static class Animations
    {
        private static KeyframeAnimation<int> Kfa(long ms, bool loop, IEnumerable<int> frames) => new KeyframeAnimation<int>(TimeSpan.FromMilliseconds(ms), loop, frames);

        public static readonly SpriteAnimationSet Player = new SpriteAnimationSet(Sprites.Alphonse)
            .AddAnimation("IdleNorth", Kfa(100, true, new[] { 6 }))
            .AddAnimation("IdleEast",  Kfa(100, true, new[] { 0 }))
            .AddAnimation("IdleSouth", Kfa(100, true, new[] { 12 }))
            .AddAnimation("IdleWest",  Kfa(100, true, new[] { 18 }))
            .AddAnimation("WalkNorth", Kfa(100, true, new[] { 7, 6, 8, 6 }))
            .AddAnimation("WalkEast",  Kfa(100, true, new[] { 1, 0, 2, 0 }))
            .AddAnimation("WalkSouth", Kfa(100, true, new[] { 13, 12, 14, 12 }))
            .AddAnimation("WalkWest",  Kfa(100, true, new[] { 19, 18, 20, 18 }));
    }
}
