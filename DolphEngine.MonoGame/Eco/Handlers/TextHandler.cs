using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.MonoGame.Eco.Components;
using Microsoft.Xna.Framework;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class TextHandler : EcosystemHandler<TextComponent, DrawComponent>
    {
        private static readonly Color DefaultColor = Color.White;

        public override void Draw(Entity entity)
        {
            var textComponent = entity.GetComponent<TextComponent>();
            var drawComponent = entity.GetComponent<DrawComponent>();

            if (textComponent.SpriteFont == null)
            {
                // Can't draw if there's no font specified
                return;
            }

            var position = Vector2.Zero;
            if (entity.TryGetComponent<PositionComponent2d>(out var positionComponent))
            {
                position = new Vector2(positionComponent.X, positionComponent.Y);
            }

            if (entity.TryGetComponent<SizeComponent2d>(out var sizeComponent))
            {
                // todo: implement wrapping
            }

            var color = textComponent.Color ?? DefaultColor;
            drawComponent.DrawDelegates.Add(sb => sb.DrawString(textComponent.SpriteFont, textComponent.Text, position, color));
        }
    }
}
