using DolphEngine.Input.Controls;
using DolphEngine.Input.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.Demo
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Color BackgroundColor = Color.CornflowerBlue;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Tower.Initialize();

            var controlEsc = new SingleButtonControl(InputKeys.KeyboardEscape);
            Tower.Keycosystem.AddReaction(controlEsc, c => c.IsPressed, c => this.Exit());

            var control1 = new SingleButtonControl(InputKeys.KeyboardA);
            Tower.Keycosystem.AddReaction(control1, c => c.IsPressed, c => this.BackgroundColor = Color.Crimson);

            var control2 = new SingleButtonControl(InputKeys.KeyboardZ);
            Tower.Keycosystem.AddReaction(control2, c => c.IsPressed, c => this.BackgroundColor = Color.DarkOliveGreen);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            Tower.Keycosystem.Update(gameTime.ElapsedGameTime.Ticks);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(this.BackgroundColor);

            base.Draw(gameTime);
        }
    }
}
