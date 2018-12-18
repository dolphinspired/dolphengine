using DolphEngine.Input.Controllers;
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
            var mouse = new StandardMouse();
            
            Tower.Keycosystem.AddControlReaction(keyboard, k => k.Escape.IsPressed, k => this.Exit());

            Tower.Keycosystem.AddControlReaction(keyboard, k => k.OemTilde.JustPressed, k => this.dl.Hidden = !this.dl.Hidden);
            Tower.Keycosystem.AddControlReaction(keyboard, k => k.F1.JustPressed, k => this.dl.PrevPage());
            Tower.Keycosystem.AddControlReaction(keyboard, k => k.F2.JustPressed, k => this.dl.NextPage());

            Tower.Keycosystem.AddControlReaction(keyboard, k => k.A.IsPressed, k => this.BackgroundColor = Color.Crimson);
            Tower.Keycosystem.AddControlReaction(keyboard, k => k.Z.IsPressed, k => this.BackgroundColor = Color.DarkOliveGreen);
            Tower.Keycosystem.AddControlReaction(mouse, m => m.PrimaryClick.IsPressed, m => this.BackgroundColor = Color.BurlyWood);
            Tower.Keycosystem.AddControlReaction(mouse, m => m.SecondaryClick.IsPressed, m => this.BackgroundColor = Color.Aquamarine);
            Tower.Keycosystem.AddControlReaction(mouse, m => m.MiddleClick.JustPressed, m => m.LeftHanded = !m.LeftHanded);

            dl = new DebugLogger
            {
                Hidden = true,
                CurrentPage = 1
            };

            dl.AddLine(1, () => $"CurrentGameTick: {CurrentTick}");

            dl.AddLine(1, 
                () => "",
                () => "Control A:",
                () => $"IsPressed: {keyboard.A.IsPressed}, LastTickPressed: {keyboard.A.LastTickPressed}, LastTickReleased: {keyboard.A.LastTickReleased}");

            dl.AddLine(1,
                () => "",
                () => "Control Z:",
                () => $"IsPressed: {keyboard.Z.IsPressed}, LastTickPressed: {keyboard.Z.LastTickPressed}, LastTickReleased: {keyboard.Z.LastTickReleased}");

            dl.AddLine(1,
                () => "",
                () => "Mouse:",
                () => $"PrimaryClick: {mouse.PrimaryClick.IsPressed}, SecondaryClick: {mouse.SecondaryClick.IsPressed}, LeftHanded: {mouse.LeftHanded}",
                () => $"X: {mouse.Cursor.X}, Y: {mouse.Cursor.Y}");

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

            this.BackgroundColor = Color.CornflowerBlue; // Reset to default color before reading inputs
            Tower.Keycosystem.Update(gameTime.TotalGameTime.Ticks);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(this.BackgroundColor);

            spriteBatch.Begin(samplerState: SamplerState.PointWrap); // disable anti-aliasing
            dl.Render(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
