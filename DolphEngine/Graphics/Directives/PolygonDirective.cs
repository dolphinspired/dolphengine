using System.Collections.Generic;

namespace DolphEngine.Graphics.Directives
{
    public class PolygonDirective : DrawDirective
    {
        public virtual ColorRGBA Color { get; set; }

        public virtual IList<Position2d> Points { get; set; }
    }
}
