using System.Collections.Generic;

namespace DolphEngine.Graphics.Directives
{
    public class PolygonDirective
    {
        // todo: Get a real color struct
        public uint Color;

        public IList<Position2d> Points;
    }
}
