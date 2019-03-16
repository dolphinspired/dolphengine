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
            this.GlobalKeycosystem
                .AddContext(ControlContexts.System(this))
                .AddContext(ControlContexts.DebugNavigation(this.Debug));

            this.Debug
                .AddControlInfo(ControlContexts.Keyboard, ControlContexts.Mouse);

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
    }
}
