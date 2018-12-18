using DolphEngine.Input.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.Demo
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

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
            var mouse = new StandardMouse();
            
            Tower.Keycosystem
                .AddControlReaction(keyboard, k => k.Escape.IsPressed, k => this.Exit());

            Tower.Keycosystem
                .AddControlReaction(keyboard, k => k.OemTilde.JustPressed, k => Tower.Debug.Hidden = !Tower.Debug.Hidden)
                .AddControlReaction(keyboard, k => k.F1.JustPressed, k => Tower.Debug.PrevPage())
                .AddControlReaction(keyboard, k => k.F2.JustPressed, k => Tower.Debug.NextPage());

            Tower.Keycosystem
                .AddControlReaction(keyboard, k => k.A.IsPressed, k => this.BackgroundColor = Color.Crimson)
                .AddControlReaction(keyboard, k => k.Z.IsPressed, k => this.BackgroundColor = Color.DarkOliveGreen)
                .AddControlReaction(mouse, m => m.PrimaryClick.IsPressed, m => this.BackgroundColor = Color.BurlyWood)
                .AddControlReaction(mouse, m => m.SecondaryClick.IsPressed, m => this.BackgroundColor = Color.Aquamarine)
                .AddControlReaction(mouse, m => m.MiddleClick.JustPressed, m => m.LeftHanded = !m.LeftHanded);
            
            Tower.Debug.AddLine(1,
                () => $"CurrentGameTick: {CurrentTick}",
                DebugLogger.EmptyLine,
                () => "Control A:",
                () => $"IsPressed: {keyboard.A.IsPressed}, LastTickPressed: {keyboard.A.LastTickPressed}, LastTickReleased: {keyboard.A.LastTickReleased}",
                DebugLogger.EmptyLine,
                () => "Control Z:",
                () => $"IsPressed: {keyboard.Z.IsPressed}, LastTickPressed: {keyboard.Z.LastTickPressed}, LastTickReleased: {keyboard.Z.LastTickReleased}",
                DebugLogger.EmptyLine,
                () => "Mouse:",
                () => $"PrimaryClick: {mouse.PrimaryClick.IsPressed}, SecondaryClick: {mouse.SecondaryClick.IsPressed}, LeftHanded: {mouse.LeftHanded}",
                () => $"X: {mouse.Cursor.X}, Y: {mouse.Cursor.Y}");

            Tower.Debug.AddLine(2, "This is page 2!");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Tower.Debug.Font = this.Content.Load<SpriteFont>("Debug");
        }

        protected override void Update(GameTime gameTime)
        {
            this.CurrentTick = gameTime.TotalGameTime.Ticks;

            this.BackgroundColor = Color.CornflowerBlue; // Reset to default color before reading inputs
            Tower.Keycosystem.Update(gameTime.TotalGameTime.Ticks);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(this.BackgroundColor);

            spriteBatch.Begin(samplerState: SamplerState.PointWrap); // disable anti-aliasing
            Tower.Debug.Render(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
