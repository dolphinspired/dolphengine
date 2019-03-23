using DolphEngine.Graphics.Animations;
using DolphEngine.Graphics.Sprites;
using System;
using System.Collections.Generic;

namespace DolphEngine.Demo
{
    public static class Animations
    {
        private static SpritesheetAnimation Sa(long ms, SpriteSheet ss, IEnumerable<int> frames) => new SpritesheetAnimation(ss, TimeSpan.FromMilliseconds(ms), true, frames);

        public static readonly AnimationLibrary Player = new AnimationLibrary()
            .AddAnimation("IdleNorth", Sa(100, Sprites.Alphonse, new[] { 6 }))
            .AddAnimation("IdleEast",  Sa(100, Sprites.Alphonse, new[] { 0 }))
            .AddAnimation("IdleSouth", Sa(100, Sprites.Alphonse, new[] { 12 }))
            .AddAnimation("IdleWest",  Sa(100, Sprites.Alphonse, new[] { 18 }))
            .AddAnimation("WalkNorth", Sa(100, Sprites.Alphonse, new[] { 7, 6, 8, 6 }))
            .AddAnimation("WalkEast",  Sa(100, Sprites.Alphonse, new[] { 1, 0, 2, 0 }))
            .AddAnimation("WalkSouth", Sa(100, Sprites.Alphonse, new[] { 13, 12, 14, 12 }))
            .AddAnimation("WalkWest",  Sa(100, Sprites.Alphonse, new[] { 19, 18, 20, 18 }));

        public static readonly AnimationLibrary Glyph = new AnimationLibrary()
            .AddAnimation("Rotate", Rotate(TimeSpan.FromSeconds(1)))
            .AddAnimation("Breathe", Breathe(TimeSpan.FromSeconds(4)));

        public static RotationAnimation Rotate(TimeSpan time) => new RotationAnimation()
            .AddKeyframeDegrees(time / 2, 180)
            .AddKeyframeDegrees(time, 360)
            .Loop();

        public static ScaleAnimation Breathe(TimeSpan time) => new ScaleAnimation()
            .AddKeyframe(time / 4, 1.5f)
            .AddKeyframe(time / 2, 1.75f)
            .AddKeyframe(time * 0.75, 1.5f)
            .Loop(time / 4);
    }
}
