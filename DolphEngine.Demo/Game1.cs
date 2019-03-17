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

        public Game1()
        {
            this.Graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            FpsCounter = new FpsCounter(60);
            FpsPosition = new Vector2(10, Graphics.PreferredBackBufferHeight - 22);
        }

        protected override void Initialize()
        {
            Tower.Initialize();

            Tower.Keycosystem
                .AddContext(ControlContexts.System(this))
                .AddContext(ControlContexts.DebugNavigation());

            Tower.DebugLogger
                .AddControlInfo(ControlContexts.Keyboard, ControlContexts.Mouse);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);

            Tower.DebugLogger.Font = this.Content.Load<SpriteFont>("Debug");
            Tower.Director.AddScene("test-map", new TestMapScene(this.Content, this.SpriteBatch, this.Graphics.PreferredBackBufferWidth, this.Graphics.PreferredBackBufferHeight));
            Tower.Director.LoadScene("test-map");
        }

        protected override void Update(GameTime gameTime)
        {
            GameTimer.Global.Advance(gameTime.ElapsedGameTime); // todo: kill this
            Tower.Timer.Advance(gameTime.ElapsedGameTime);
            
            Tower.Keycosystem.Update(gameTime.TotalGameTime);
            Tower.Director.CurrentScene.Update(gameTime.TotalGameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Tower.Director.CurrentScene.Draw();
            Tower.DebugLogger.Render(SpriteBatch);
            
            SpriteBatch.Begin();
            SpriteBatch.DrawString(Tower.DebugLogger.Font, $"FPS: {FpsCounter.Update():0.0}", FpsPosition, Tower.DebugLogger.FontColor);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
