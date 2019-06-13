using System.Collections.Generic;

namespace DolphEngine.Graphics.Directives
{
    public class PolygonDirective : DrawDirective
    {
        // todo: Get a real color struct
        public ColorRGBA Color;

        public IList<Position2d> Points;
    }
}
