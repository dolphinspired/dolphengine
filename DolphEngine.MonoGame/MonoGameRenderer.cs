using DolphEngine.Graphics;
using DolphEngine.Graphics.Directives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace DolphEngine.MonoGame
{
    public class MonoGameRenderer : DirectiveRenderer
    {
        private const float FZero = 0.000f;
        private const float FOne = 1.000f;

        protected readonly SpriteBatch SpriteBatch;
        protected readonly ContentManager Content;

        private readonly Texture2D _pixelTexture;

        public Color BackgroundColor = Color.CornflowerBlue;
        public bool ClearFrame = true;

        public MonoGameRenderer(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.SpriteBatch = spriteBatch;
            this.Content = contentManager;

            this.AddRenderer<SpriteDirective>(this.DrawSprite)
                .AddRenderer<TextDirective>(this.DrawText)
                .AddRenderer<PolygonDirective>(this.DrawPolygon);

            this._pixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            this._pixelTexture.SetData(new Color[] { Color.White });
        }

        public override bool OnBeforeRenderView(Viewport2d viewport)
        {
            var translation = GetCameraTranslation(viewport);

            this.SpriteBatch.Begin(
                samplerState: SamplerState.PointWrap, // disable anti-aliasing
                transformMatrix: translation);

            if (this.ClearFrame)
            {
                // todo: this probably doesn't work with multiple viewports on one screen
                this.SpriteBatch.GraphicsDevice.Clear(this.BackgroundColor);
            }
            return true;
        }

        private static Matrix GetCameraTranslation(Viewport2d viewport)
        {
            if (viewport.Focus != null)
            {
                viewport.Space.MoveTo(viewport.Focus());
            }

            var zoomDiffX = (viewport.Space.Width * viewport.Zoom) - viewport.Space.Width;
            var zoomDiffY = (viewport.Space.Height * viewport.Zoom) - viewport.Space.Height;

            // This formula adapted from: https://roguesharp.wordpress.com/2014/07/13/tutorial-5-creating-a-2d-camera-with-pan-and-zoom-in-monogame/
            var translation =
                Matrix.CreateTranslation(-viewport.Space.TopLeft.X, -viewport.Space.TopLeft.Y, FZero) *
                Matrix.CreateTranslation(viewport.Pan.X, viewport.Pan.Y, FZero) *
                //Matrix.CreateRotationZ(cameraTranslation.Rotation) *
                Matrix.CreateScale(viewport.Zoom, viewport.Zoom, FOne) *
                Matrix.CreateTranslation(-zoomDiffX / 2, -zoomDiffY / 2, FZero); // Keep the camera centered after zoom

            return translation;
        }

        public override void OnAfterRenderView(Viewport2d viewport)
        {
            this.SpriteBatch.End();
        }

        #region Directive handlers
        
        private void DrawSprite(SpriteDirective sprite)
        {
            var texture = this.Content.Load<Texture2D>(sprite.Asset);
            var dest = new Rectangle(sprite.Destination.ToPoint(), sprite.Size.ToPoint());
            var color = sprite.Color.HasValue ? sprite.Color.Value.ToColor() : Color.White;
            this.SpriteBatch.Draw(texture, dest, sprite.Source.ToRectangle(), color, sprite.Rotation, sprite.Origin.ToVector2(), SpriteEffects.None, 0);
        }

        private void DrawText(TextDirective text)
        {
            var font = this.Content.Load<SpriteFont>(text.FontAssetName);
            this.SpriteBatch.DrawString(font, text.Text, text.Destination.ToVector2(), text.Color.ToColor());
        }

        private void DrawPolygon(PolygonDirective poly)
        {
            if (poly?.Points == null || poly.Points.Count < 2)
            {
                return;
            }

            var color = poly.Color.ToColor();

            Vector2 start = poly.Points[0].ToVector2();
            Vector2 end;

            foreach (var pos in poly.Points.Skip(1))
            {
                end = pos.ToVector2();

                // Adapted from: https://gamedev.stackexchange.com/a/44016
                var edge = end - start;
                var angle = (float)Math.Atan2(edge.Y, edge.X);
                // todo: there seems to be a rounding error causing poly lines to be off by 1px in some cases. Investigate this
                this.SpriteBatch.Draw(this._pixelTexture, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1), null, color, angle, Vector2.Zero, SpriteEffects.None, 0);

                start = end;
            }
        }

        #endregion
    }
}
