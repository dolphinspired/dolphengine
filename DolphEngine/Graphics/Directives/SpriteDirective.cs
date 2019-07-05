namespace DolphEngine.Graphics.Directives
{
    public class SpriteDirective : DrawDirective
    {
        public virtual string Asset { get; set; }

        public virtual Rect2d Source { get; set; }

        public virtual Size2d Size { get; set; }

        public virtual float Rotation { get; set; }

        public virtual Vector2d Origin { get; set; }

        public virtual ColorRGBA? Color { get; set; }
    }
}
