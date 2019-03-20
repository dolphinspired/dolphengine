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

        public static readonly SpriteAnimationSet Glyph = new SpriteAnimationSet(Sprites.Glyphs)
            .AddTransform("Rotate", Rotate(TimeSpan.FromSeconds(1)))
            .AddTransform("Breathe", Breathe(TimeSpan.FromSeconds(4)));

        public static KeyframeAnimation<Transform2d> Rotate(TimeSpan time) => new TransformAnimation()
            .AddKeyframe(time / 2, new Transform2d(0, 0, 1, 1, (float)Math.PI))
            .AddKeyframe(time, new Transform2d(0, 0, 1, 1, (float)(2 * Math.PI)))
            .Loop();

        public static KeyframeAnimation<Transform2d> Breathe(TimeSpan time) => new TransformAnimation()
            .AddKeyframe(time / 4, new Transform2d(0, 0, 1.5f, 1.5f, 0))
            .AddKeyframe(time / 2, new Transform2d(0, 0, 1.75f, 1.75f, 0))
            .AddKeyframe(time * 0.75, new Transform2d(0, 0, 1.5f, 1.5f, 0))
            .Loop(time / 4);
    }
}
