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

            draw.Directives.Add(new TextDirective
            {
                FontAssetName = text.FontAssetName,
                Destination = entity.Space.GetOriginPosition(),
                Text = text.Text,
                Color = text.Color
            });
        }
    }
}
