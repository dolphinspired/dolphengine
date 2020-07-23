namespace DolphEngine.Graphics.Directives
{
    public class SpriteDirective : DrawDirective, ISize2d
    {
        public virtual string Asset { get; set; }

        public virtual Rect2d Source { get; set; }

        public virtual float Width { get; set; }

        public virtual float Height { get; set; }

        public virtual float Rotation { get; set; }

        public virtual Vector2d Origin { get; set; }

        public virtual ColorRGBA? Color { get; set; }
    }
}
