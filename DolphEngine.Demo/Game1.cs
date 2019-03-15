using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.MonoGame.Input;
using DolphEngine.Scenery;
using DolphEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.Demo
{
    public class Game1 : Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;

        private static FpsCounter FpsCounter;
        private static Vector2 FpsPosition;

        protected readonly Director Director;
        protected readonly DebugLogger Debug;
        protected readonly Keycosystem GlobalKeycosystem;

        public Game1()
        {
            this.Graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            FpsCounter = new FpsCounter(60);
            FpsPosition = new Vector2(10, Graphics.PreferredBackBufferHeight - 22);

            this.Director = new Director();
            this.Debug = new DebugLogger { Hidden = true };
            this.GlobalKeycosystem = new Keycosystem(new MonoGameObserver().UseKeyboard().UseMouse());
        }

        protected override void Initialize()
        {
            this.InitializeControls();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.Debug.Font = this.Content.Load<SpriteFont>("Debug");

            this.Director.AddScene("test-map", new TestMapScene(this.Content, this.SpriteBatch, this.Debug, this.Graphics.PreferredBackBufferWidth, this.Graphics.PreferredBackBufferHeight));
            this.Director.LoadScene("test-map");
        }

        protected override void Update(GameTime gameTime)
        {
            GameTimer.Global.Advance(gameTime.ElapsedGameTime);
            
            this.GlobalKeycosystem.Update();
            this.Director.CurrentScene.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.Director.CurrentScene.Draw();
            this.Debug.Render(SpriteBatch);
            
            SpriteBatch.Begin();
            SpriteBatch.DrawString(this.Debug.Font, $"FPS: {FpsCounter.Update():0.0}", FpsPosition, this.Debug.FontColor);
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void InitializeControls()
        {
            var keyboard = new StandardKeyboard();
            var mouse = new StandardMouse();

            this.GlobalKeycosystem
                .AddControlReaction(keyboard, k => k.Escape.IsPressed, k => this.Exit());

            this.GlobalKeycosystem
                .AddControlReaction(keyboard, k => k.OemTilde.JustPressed, k => this.Debug.Hidden = !this.Debug.Hidden)
                .AddControlReaction(keyboard, k => k.F1.JustPressed, k => this.Debug.PrevPage())
                .AddControlReaction(keyboard, k => k.F2.JustPressed, k => this.Debug.NextPage());

            this.Debug.AddPage(
                () => $"GameTime: {GameTimer.Global.Total:g}",
                DebugLogger.EmptyLine,
                () => "Control A:",
                () => $"IsPressed: {keyboard.A.IsPressed}, LastTickPressed: {keyboard.A.LastTickPressed}, LastTickReleased: {keyboard.A.LastTickReleased}",
                DebugLogger.EmptyLine,
                () => "Control Z:",
                () => $"IsPressed: {keyboard.Z.IsPressed}, LastTickPressed: {keyboard.Z.LastTickPressed}, LastTickReleased: {keyboard.Z.LastTickReleased}",
                DebugLogger.EmptyLine,
                () => "Arrow keys:",
                () => $"IsPressed: {keyboard.ArrowKeys.IsPressed}, LastTickPressed: {keyboard.ArrowKeys.LastTickPressed}, LastTickReleased: {keyboard.ArrowKeys.LastTickReleased}",
                () => $"Direction: {keyboard.ArrowKeys.Direction}, LastTickDirectionChanged: {keyboard.ArrowKeys.LastTickDirectionChanged}, DirectionHeld: {keyboard.ArrowKeys.DurationDirectionHeld}",
                DebugLogger.EmptyLine,
                () => "Mouse:",
                () => $"PrimaryClick: {mouse.PrimaryClick.IsPressed}, SecondaryClick: {mouse.SecondaryClick.IsPressed}, LeftHanded: {mouse.LeftHanded}",
                () => $"X: {mouse.Cursor.X}, Y: {mouse.Cursor.Y}",
                () => $"Scroll X: {mouse.Scroll.X}, Scroll Y: {mouse.Scroll.Y}, Scroll Click: {mouse.MiddleClick.IsPressed}"
            );
        }
    }
}
