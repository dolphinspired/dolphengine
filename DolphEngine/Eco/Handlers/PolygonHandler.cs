﻿using DolphEngine.Eco.Components;
using DolphEngine.Graphics.Directives;
using System.Linq;

namespace DolphEngine.Eco.Handlers
{
    public class PolygonHandler : EcosystemHandler<PolygonComponent>
    {
        public override void Draw(Entity entity)
        {
            var poly = entity.GetComponent<PolygonComponent>();

            entity.SetDirective<PolygonDirective>("simple-polygon", pd =>
            {
                pd.Color = poly.Color;
                pd.Points = poly.Polygon.Points.Select(x => x.Shift(entity.Space.GetOriginPosition().ToVector())).ToList();
            });
        }
    }
}
