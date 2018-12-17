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

        SpriteFont font;
        DebugLogger dl;

        Color BackgroundColor = Color.CornflowerBlue;

        private long CurrentTick;

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
            Tower.Keycosystem.AddControlReaction(controlEsc, c => c.IsPressed, c => this.Exit());

            var control1 = new SingleButtonControl(InputKeys.KeyboardA);
            Tower.Keycosystem.AddControlReaction(control1, c => c.JustPressed, c => this.BackgroundColor = Color.Crimson);
            Tower.Keycosystem.AddControlReaction(control1, c => c.JustReleased, c => this.BackgroundColor = Color.CornflowerBlue);

            var control2 = new SingleButtonControl(InputKeys.KeyboardZ);
            Tower.Keycosystem.AddControlReaction(control2, c => c.JustPressed, c => this.BackgroundColor = Color.DarkOliveGreen);
            Tower.Keycosystem.AddControlReaction(control2, c => c.JustReleased, c => this.BackgroundColor = Color.CornflowerBlue);

            dl = new DebugLogger();
            dl.CurrentPage = 1;

            dl.AddLine(1, () => $"CurrentGameTick: {CurrentTick}");

            dl.AddLine(1, 
                () => "",
                () => "Control A:",
                () => $"IsPressed: {control1.IsPressed}, LastTickPressed: {control1.LastTickPressed}, LastTickReleased: {control1.LastTickReleased}");

            dl.AddLine(1,
                () => "",
                () => "Control Z:",
                () => $"IsPressed: {control2.IsPressed}, LastTickPressed: {control2.LastTickPressed}, LastTickReleased: {control2.LastTickReleased}");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = this.Content.Load<SpriteFont>("Debug");

            dl.Font = font;

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            this.CurrentTick = gameTime.TotalGameTime.Ticks;
            Tower.Keycosystem.Update(gameTime.TotalGameTime.Ticks);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(this.BackgroundColor);

            spriteBatch.Begin();
            dl.Render(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
