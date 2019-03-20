using System;
using System.Collections.Generic;

namespace DolphEngine.Graphics.Sprites
{
    public class SpriteSheet
    {
        #region Constructors
        
        public SpriteSheet(string name, List<Rect2d> frames)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{nameof(SpriteSheet)} name is required.");
            }

            if (frames.Count == 0)
            {
                throw new InvalidOperationException($"Cannot create a {nameof(SpriteSheet)} with no frames.");
            }

            this.Name = name;
            this.Frames = frames;
        }

        public SpriteSheet(string name, int width, int height, int columns, int rows)
            : this(name, new Size2d(width, height), Position2d.Zero, columns, rows, 0, 0)
        {
        }

        public SpriteSheet(string name, Size2d size, int columns, int rows)
            : this(name, size, Position2d.Zero, columns, rows, 0, 0)
        {
        }

        public SpriteSheet(string name, Size2d size, Position2d origin, int columns, int rows)
            : this(name, size, origin, columns, rows, 0, 0)
        {
        }

        public SpriteSheet(string name, Size2d size, Position2d origin, int columns, int rows, int columnPadding, int rowPadding)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{nameof(SpriteSheet)} name is required.");
            }

            this.Name = name;
            var frames = new List<Rect2d>(columns * rows);

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    float x = origin.X + (c * size.Width) + (c * columnPadding);
                    float y = origin.Y + (r * size.Height) + (r * rowPadding);
                    frames.Add(new Rect2d(x, y, size.Width, size.Height));
                }
            }

            if (frames.Count == 0)
            {
                throw new InvalidOperationException($"Cannot create a {nameof(SpriteSheet)} with no frames.");
            }

            this.Frames = frames;
        }

        #endregion

        #region Properties

        public readonly string Name;

        public readonly IReadOnlyList<Rect2d> Frames;

        #endregion
    }
}
