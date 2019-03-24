using System.Collections.Generic;

namespace DolphEngine.Eco.Components
{
    public class PolygonComponent : Component
    {
        // todo: get a real color struct
        public uint Color;

        /// <summary>
        /// A list of relative brushstrokes to get to the desired shape
        /// </summary>
        public IList<Vector2d> Points;

        /// <summary>
        /// If true, the last point will be connected back to the first
        /// </summary>
        public bool Close;
    }
}
