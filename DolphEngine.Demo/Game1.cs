using DolphEngine.Input.Controllers;
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

            var keyboard = new StandardKeyboard();
            
            Tower.Keycosystem.AddControlReaction(keyboard, c => c.Escape.IsPressed, c => this.Exit());
            
            Tower.Keycosystem.AddControlReaction(keyboard, c => c.A.JustPressed, c => this.BackgroundColor = Color.Crimson);
            Tower.Keycosystem.AddControlReaction(keyboard, c => c.A.JustReleased, c => this.BackgroundColor = Color.CornflowerBlue);
            
            Tower.Keycosystem.AddControlReaction(keyboard, c => c.Z.JustPressed, c => this.BackgroundColor = Color.DarkOliveGreen);
            Tower.Keycosystem.AddControlReaction(keyboard, c => c.Z.JustReleased, c => this.BackgroundColor = Color.CornflowerBlue);

            dl = new DebugLogger();
            dl.CurrentPage = 1;

            dl.AddLine(1, () => $"CurrentGameTick: {CurrentTick}");

            dl.AddLine(1, 
                () => "",
                () => "Control A:",
                () => $"IsPressed: {keyboard.A.IsPressed}, LastTickPressed: {keyboard.A.LastTickPressed}, LastTickReleased: {keyboard.A.LastTickReleased}");

            dl.AddLine(1,
                () => "",
                () => "Control Z:",
                () => $"IsPressed: {keyboard.Z.IsPressed}, LastTickPressed: {keyboard.Z.LastTickPressed}, LastTickReleased: {keyboard.Z.LastTickReleased}");

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
