using DolphEngine.Graphics.Sprites;

namespace DolphEngine.Demo
{
    public static class Animations
    {
        public static SpriteAnimationLibrary Player = new SpriteAnimationLibrary
        (
            new SpriteAnimation("IdleNorth", Sprites.Alphonse, new[] { 6 }),
            new SpriteAnimation("IdleEast",  Sprites.Alphonse, new[] { 0 }),
            new SpriteAnimation("IdleSouth", Sprites.Alphonse, new[] { 12 }),
            new SpriteAnimation("IdleWest",  Sprites.Alphonse, new[] { 18 }),
            new SpriteAnimation("WalkNorth", Sprites.Alphonse, new[] { 7, 6, 8, 6 }),
            new SpriteAnimation("WalkEast",  Sprites.Alphonse, new[] { 1, 0, 2, 0 }),
            new SpriteAnimation("WalkSouth", Sprites.Alphonse, new[] { 13, 12, 14, 12 }),
            new SpriteAnimation("WalkWest",  Sprites.Alphonse, new[] { 19, 18, 20, 18 })
        );
    }
}
