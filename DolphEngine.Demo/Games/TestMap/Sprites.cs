using DolphEngine.Graphics.Sprites;
using System.Collections.Generic;

namespace DolphEngine.Demo.Games.TestMap
{
    public static class Sprites
    {
        public static SpriteSheet Alphonse = new SpriteSheet("Assets/Alphonse", 32, 64, 6, 4);

        public static SpriteSheet Tiles = new SpriteSheet("Assets/iso_tiles_32_single_v3", 64, 49, 4, 4);

        public static SpriteSheet Glyphs = new SpriteSheet("Assets/glyphs", new List<Rect2d>
        {
            new Rect2d(0, 0, 21, 11),
            new Rect2d(21, 0, 7, 11),
            new Rect2d(28, 0, 11, 11)
        });
    }
}
