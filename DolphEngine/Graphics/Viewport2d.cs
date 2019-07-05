using System;

namespace DolphEngine.Graphics
{
    public class Viewport2d
    {
        public Viewport2d() { }

        public Viewport2d(Rect2d space)
        {
            this.Space = space;
        }

        public Rect2d Space;

        public float Zoom = 1.000f;

        public Vector2d Pan;

        public Func<Position2d> Focus;

        #region Object overrides

        public override string ToString()
        {
            return $"{{ space: {this.Space}, zoom: {this.Zoom}, pan: {this.Pan}, focus: {this.Focus?.Invoke().ToString() ?? "null"} }}";
        }

        #endregion
    }
}
