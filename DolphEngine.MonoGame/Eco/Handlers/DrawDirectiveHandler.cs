using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Graphics.Directives;
using DolphEngine.MonoGame.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public partial class DrawDirectiveHandler : EcosystemHandler<DrawComponent>
    {
        private const float FZero = 0.000f;
        private const float FOne = 1.000f;

        public SpriteBatch SpriteBatch;
        public ContentManager Content;
        public Entity Camera;

        public Color BackgroundColor = Color.CornflowerBlue;

        public DrawDirectiveHandler(SpriteBatch spriteBatch, ContentManager contentManager, Entity camera)
        {
            this.SpriteBatch = spriteBatch;
            this.Content = contentManager;
            this.Camera = camera;
        }

        public override void Draw(IEnumerable<Entity> entities)
        {
            var cameraSize = this.Camera.GetComponentOrDefault<SizeComponent2d>();

            if (cameraSize == null || cameraSize.Width <= 0 || cameraSize.Height <= 0)
            {
                // If the camera is zero-size, nothing can be drawn, but we still need to clear out the last frame
                this.SpriteBatch.Begin();
                this.SpriteBatch.GraphicsDevice.Clear(this.BackgroundColor);
                this.SpriteBatch.End();
                return;
            }

            var cameraPosition = this.Camera.GetComponentOrDefault(new PositionComponent2d(0, 0));
            var cameraTranslation = this.Camera.GetComponentOrDefault(new TransformComponent2d(0, 0, 0, 0, FZero));

            Position2d cameraFocus;
            if (this.Camera.TryGetComponent<SingleTargetComponent>(out var focus)               // If the camera has a focusing component
                && focus.Target != null                                                         // And a target is specified
                && focus.Target.TryGetComponent<PositionComponent2d>(out var targetPosition))   // And that target has a position
            {
                // Then calculate the top-left point of the camera that would "center" it on the target's position
                cameraFocus = new Position2d(targetPosition.X - cameraSize.Width / 2, targetPosition.Y - cameraSize.Height / 2);
            }
            else
            {
                cameraFocus = Position2d.Zero;
            }

            // This formula adapted from: https://roguesharp.wordpress.com/2014/07/13/tutorial-5-creating-a-2d-camera-with-pan-and-zoom-in-monogame/
            var translation =
                Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0) *
                Matrix.CreateRotationZ(cameraTranslation.Rotation) *
                Matrix.CreateScale(cameraTranslation.Scale.X, cameraTranslation.Scale.Y, FOne) *
                Matrix.CreateTranslation(-cameraFocus.X, -cameraFocus.Y, 0) *
                Matrix.CreateTranslation(cameraTranslation.Offset.X, cameraTranslation.Offset.Y, FZero);

            this.SpriteBatch.Begin(
                samplerState: SamplerState.PointWrap, // disable anti-aliasing
                transformMatrix: translation);

            this.SpriteBatch.GraphicsDevice.Clear(this.BackgroundColor);

            foreach (var entity in entities)
            {
                var drawDirectives = entity.GetComponent<DrawComponent>().Directives;

                if (drawDirectives.Count == 0)
                {
                    // There is nothing to draw for this entity
                    continue;
                }

                foreach (var directive in drawDirectives)
                {
                    // Invoke the draw action using the camera-transformed SpriteBatch
                    this.RouteDirective(directive);
                }

                // Remove all delegates once they've been drawn so that they won't be drawn on the next frame
                drawDirectives.Clear();
            }

            this.SpriteBatch.End();
        }

        private void RouteDirective(object directive)
        {
            var type = directive.GetType();
            if (type == typeof(SpriteDirective))
            {
                var sd = (SpriteDirective)directive;
                var texture = this.Content.Load<Texture2D>(sd.Asset);
                this.SpriteBatch.Draw(texture, sd.Destination.ToRectangle(), sd.Source.ToRectangle(), Color.White, sd.Rotation, Vector2.Zero, SpriteEffects.None, 0);
            }
            if (type == typeof(TextDirective))
            {
                var td = (TextDirective)directive;
                var font = this.Content.Load<SpriteFont>(td.FontAssetName);
                this.SpriteBatch.DrawString(font, td.Text, td.Destination.ToVector2(), Color.White);
            }
        }
    }
}
