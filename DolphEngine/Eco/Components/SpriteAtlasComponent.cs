using System.Collections.Generic;

namespace DolphEngine.Eco.Components
{
    public class SpriteAtlasComponent : Component
    {
        public SpriteAtlasComponent()
        {
        }

        public SpriteAtlasComponent(List<Rect2d> frames)
        {
            this._frames = frames;
        }

        public SpriteAtlasComponent(Size2d frameSize, int columns, int rows)
            : this(frameSize, Position2d.Zero, columns, rows, 0, 0)
        {
        }

        public SpriteAtlasComponent(Size2d frameSize, int columns, int rows, int columnPadding, int rowPadding)
            : this(frameSize, Position2d.Zero, columns, rows, columnPadding, rowPadding)
        {
        }

        public SpriteAtlasComponent(Size2d frameSize, Position2d origin, int columns, int rows)
            : this(frameSize, origin, columns, rows, 0, 0)
        {
        }

        public SpriteAtlasComponent(Size2d frameSize, Position2d origin, int columns, int rows, int columnPadding, int rowPadding)
        {
            var frames = new List<Rect2d>(columns * rows);

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < columns; c++)
                {
                    int x = origin.X + (c * frameSize.Width) + (c * columnPadding);
                    int y = origin.Y + (r * frameSize.Height) + (r * rowPadding);
                    frames.Add(new Rect2d(x, y, frameSize.Width, frameSize.Height));
                }
            }

            this._frames = frames;
        }

        public int Index;

        public List<Rect2d> Frames
        {
            get => this._frames ?? (this._frames = new List<Rect2d>(0));
            set => this._frames = value;
        }
        private List<Rect2d> _frames;
    }
}
