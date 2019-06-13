using DolphEngine.Eco.Components;
using DolphEngine.Graphics.Directives;

namespace DolphEngine.Eco.Handlers
{
    public class TextHandler : EcosystemHandler<TextComponent>
    {
        public override void Draw(Entity entity)
        {
            var text = entity.GetComponent<TextComponent>();

            if (text.FontAssetName == null)
            {
                // Can't draw if there's no font specified
                return;
            }

            entity.SetDirective<TextDirective>("simple-text", td =>
            {
                td.FontAssetName = text.FontAssetName;
                td.Destination = entity.Space.GetOriginPosition();
                td.Text = text.Text;
                td.Color = text.Color;
            });
        }
    }
}
