using DolphEngine.Eco.Components;
using DolphEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Eco.Handlers
{
    public class DrawHandler : EcosystemHandler<DrawComponent>
    {
        protected readonly DirectiveRenderer Renderer;

        public DrawHandler(DirectiveRenderer renderer)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException(nameof(renderer));
            }

            this.Renderer = renderer;
        }

        public override void Draw(IEnumerable<Entity> entities)
        {
            var allComponents = entities.Select(e => e.GetComponent<DrawComponent>());
            var allDirectives = allComponents.SelectMany(c => c.Directives);

            this.Renderer.Draw(allDirectives);

            foreach (var component in allComponents)
            {
                // Clear out all draw directives each frame
                component.Directives.Clear();
            }
        }
    }
}
