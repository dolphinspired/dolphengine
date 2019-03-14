using DolphEngine.Eco.Components;
using DolphEngine.Graphics.Directives;

namespace DolphEngine.Eco.Handlers
{
    public class TextHandler : EcosystemHandler<TextComponent, DrawComponent>
    {
        public override void Draw(Entity entity)
        {
            var text = entity.GetComponent<TextComponent>();
            var draw = entity.GetComponent<DrawComponent>();

            if (text.FontAssetName == null)
            {
                // Can't draw if there's no font specified
                return;
            }

            // Default: Draw at 0,0
            var pos = Position2d.Zero;
            if (entity.TryGetComponent<PositionComponent2d>(out var position))
            {
                // If the entity has a position, draw it there
                pos = new Position2d(position.X, position.Y);
            }

            if (entity.TryGetComponent<SizeComponent2d>(out var size))
            {
                // todo: implement wrapping
            }

            draw.Directives.Add(new TextDirective
            {
                FontAssetName = text.FontAssetName,
                Destination = pos,
                Text = text.Text
            });
        }
    }
}
