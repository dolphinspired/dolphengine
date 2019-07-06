using System;

namespace DolphEngine.Graphics
{
    public class Viewport2d : Rect2dBase
    {
        public Viewport2d() { }

        public Viewport2d(Rect2d rect) : base(rect) { }

        public float Zoom = 1.000f;

        public Vector2d Pan;

        public Func<Position2d> Focus;

        #region Object overrides

        public override string ToString()
        {
            return $"{{ rect: {this.Rect}, zoom: {this.Zoom}, pan: {this.Pan}, focus: {this.Focus?.Invoke().ToString() ?? "null"} }}";
        }

        #endregion
    }
}
