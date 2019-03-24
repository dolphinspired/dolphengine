using DolphEngine.Eco.Components;
using DolphEngine.Graphics.Directives;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Eco.Handlers
{
    public class PolygonHandler : EcosystemHandler<PolygonComponent, DrawComponent>
    {
        public override void Draw(Entity entity)
        {
            var poly = entity.GetComponent<PolygonComponent>();
            var draw = entity.GetComponent<DrawComponent>();

            if (poly.Points == null || poly.Points.Count < 2)
            {
                return;
            }

            

            Position2d first = entity.Space.Position + poly.Points[0];
            var points = new List<Position2d>(poly.Points.Count + 1) { first };
            Position2d prev = first;
            Position2d next;

            foreach (var vector in poly.Points.Skip(1))
            {
                next = prev + vector;
                points.Add(next);
                prev = next;
            }

            if (poly.Close)
            {
                points.Add(first);
            }

            draw.Directives.Add(new PolygonDirective
            {
                Color = poly.Color,
                Points = points
            });
        }
    }
}
