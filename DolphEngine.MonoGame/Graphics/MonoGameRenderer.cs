using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Graphics;
using DolphEngine.Graphics.Directives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DolphEngine.MonoGame.Graphics
{
    public class MonoGameRenderer : DirectiveRenderer
    {
        private const float FZero = 0.000f;
        private const float FOne = 1.000f;

        protected readonly SpriteBatch SpriteBatch;
        protected readonly ContentManager Content;
        protected readonly Entity Camera;

        public Color BackgroundColor = Color.CornflowerBlue;

        public MonoGameRenderer(SpriteBatch spriteBatch, ContentManager contentManager, Entity camera)
        {
            this.SpriteBatch = spriteBatch;
            this.Content = contentManager;
            this.Camera = camera;

            this.AddRenderer<SpriteDirective>(this.DrawSprite)
                .AddRenderer<TextDirective>(this.DrawText);
        }

        public override bool OnBeforeDraw()
        {
            var cameraSize = this.Camera.GetComponentOrDefault<SizeComponent2d>();

            if (cameraSize == null || cameraSize.Width <= 0 || cameraSize.Height <= 0)
            {
                // If the camera is zero-size, nothing can be drawn, but we still need to clear out the last frame
                this.SpriteBatch.Begin();
                this.SpriteBatch.GraphicsDevice.Clear(this.BackgroundColor);
                this.SpriteBatch.End();
                return false;
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
            return true;
        }

        public override void OnAfterDraw()
        {
            this.SpriteBatch.End();
        }

        #region Directive handlers

        private void DrawStuff(IEnumerable<int> coll)
        {

        }

        private void DrawSprite(SpriteDirective sprite)
        {
            var texture = this.Content.Load<Texture2D>(sprite.Asset);
            this.SpriteBatch.Draw(texture, sprite.Destination.ToRectangle(), sprite.Source.ToRectangle(), Color.White, sprite.Rotation, Vector2.Zero, SpriteEffects.None, 0);
        }

        private void DrawText(TextDirective text)
        {
            var font = this.Content.Load<SpriteFont>(text.FontAssetName);
            this.SpriteBatch.DrawString(font, text.Text, text.Destination.ToVector2(), Color.White);
        }

        #endregion
    }
}
