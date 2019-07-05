namespace DolphEngine.Graphics.Directives
{
    public class TextDirective : DrawDirective
    {
        public virtual string Text { get; set; }

        public virtual string FontAssetName { get; set; }

        public virtual ColorRGBA Color { get; set; }
    }
}
