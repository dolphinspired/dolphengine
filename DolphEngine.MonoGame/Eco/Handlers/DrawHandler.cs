using System.Collections.Generic;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.MonoGame.Eco.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.MonoGame.Eco.Handlers
{
    public class DrawHandler : EcosystemHandler<DrawComponent>
    {
        private const float FZero = 0.000f;
        private const float FOne = 1.000f;

        public DrawHandler(SpriteBatch sb, Entity camera)
        {
            this.SpriteBatch = sb;
            this.Camera = camera;
        }

        public SpriteBatch SpriteBatch;

        public Entity Camera;

        public Color BackgroundColor = Color.CornflowerBlue;

        public override void Draw(IEnumerable<Entity> entities)
        {
            var cameraSize = this.Camera.GetComponentOrDefault<SizeComponent2d>();
            
            if (cameraSize == null || cameraSize.Width <= 0 || cameraSize.Height <= 0)
            {
                // If the camera is zero-size, nothing can be drawn
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
                var drawDelegates = entity.GetComponent<DrawComponent>().DrawDelegates;

                if (drawDelegates == null || drawDelegates.Count == 0)
                {
                    // There is nothing to draw for this entity
                    continue;
                }

                foreach (var drawDelegate in drawDelegates)
                {
                    // Invoke the draw action using the camera-transformed SpriteBatch
                    drawDelegate(this.SpriteBatch);
                }

                // Remove all delegates once they've been drawn so that they won't be drawn on the next frame
                drawDelegates.Clear();
            }

            this.SpriteBatch.End();
        }
    }
}
