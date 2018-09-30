using Microsoft.Xna.Framework;

namespace TacticsGame.Engine.Eco.Components
{
    public sealed class PositionComponent : Component
    {
        public int X
        {
            get => this._x;            
            set
            {
                this._x = value;
                this._rect = null;
            }
        }
        private int _x;

        public int Y
        {
            get => this._y;
            set
            {
                this._y = value;
                this._rect = null;
            }
        }
        private int _y;

        public int Width
        {
            get => this._width;
            set
            {
                this._width = value;
                this._rect = null;
            }
        }
        private int _width;

        public int Height
        {
            get => this._height;
            set
            {
                this._height = value;
                this._rect = null;
            }
        }
        private int _height;

        public Rectangle Rect => this._rect.HasValue ? this._rect.Value : (this._rect = new Rectangle(this._x, this._y, 0, 0)).Value;
        private Rectangle? _rect;
    }
}