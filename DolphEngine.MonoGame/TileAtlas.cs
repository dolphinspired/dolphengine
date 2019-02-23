using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DolphEngine.MonoGame
{
    public class TileAtlas
    {
        public static TileAtlas FromSpritesheet(Texture2D texture, int columns, int rows)
        {
            var tileset = new TileAtlas();

            var frameWidth = texture.Bounds.Width / columns;
            var frameHeight = texture.Bounds.Height / rows;

            tileset.Texture = texture;
            tileset.Frames = new List<Rectangle>(columns * rows);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    var xPos = x * frameWidth;
                    var yPos = y * frameHeight;

                    tileset.Frames.Add(new Rectangle(xPos, yPos, frameWidth, frameHeight));
                }
            }

            return tileset;
        }

        public Texture2D Texture;

        public List<Rectangle> Frames;
    }
}
