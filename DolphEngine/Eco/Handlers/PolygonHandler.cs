using DolphEngine.Eco.Components;
using DolphEngine.Graphics.Directives;
using System.Linq;

namespace DolphEngine.Eco.Handlers
{
    public class PolygonHandler : EcosystemHandler<PolygonComponent, DrawComponent>
    {
        public override void Draw(Entity entity)
        {
            var poly = entity.GetComponent<PolygonComponent>();
            var draw = entity.GetComponent<DrawComponent>();

            draw.Directives.Add(new PolygonDirective
            {
                Color = poly.Color,
                Points = poly.Polygon.Points.Select(x => x.Shift(entity.Space.GetOriginPosition().ToVector())).ToList()
            });
        }
    }
}
