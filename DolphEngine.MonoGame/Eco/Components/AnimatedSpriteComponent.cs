using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DolphEngine.MonoGame.Eco.Components
{
    public class AnimatedSpriteComponent : SpriteComponent
    {
        public static AnimatedSpriteComponent BuildFromSpritesheet(Texture2D texture, int columns, int rows)
        {
            var component = new AnimatedSpriteComponent();

            var frameWidth = texture.Bounds.Width / columns;
            var frameHeight = texture.Bounds.Height / rows;

            component.Texture = texture;
            component.Size = new Size2d(frameWidth, frameHeight);
            component.Frames = new List<Rectangle>(columns * rows);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    var xPos = x * frameWidth;
                    var yPos = y * frameHeight;

                    component.Frames.Add(new Rectangle(xPos, yPos, frameWidth, frameHeight));
                }
            }

            return component;
        }

        public long StartingTick;

        public long DurationPerFrame;

        public List<int> Sequence;

        public List<Rectangle> Frames;

        public AnimatedSpriteBehavior Behavior;
    }

    public enum AnimatedSpriteBehavior
    {
        Loop = 0,

        HoldOnLastFrame = 1,

        DisappearAfterLastFrame = 2
    }
}
