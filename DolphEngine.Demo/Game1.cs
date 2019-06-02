using DolphEngine.Demo.Games.DogGame;
using DolphEngine.Demo.Games.InputTester;
using DolphEngine.Demo.Games.TestMap;
using DolphEngine.DI;
using DolphEngine.Eco;
using DolphEngine.Eco.Entities;
using DolphEngine.Graphics;
using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.MonoGame;
using DolphEngine.Scenery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DolphEngine.Demo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;

        // Use the DolphEngine implementation of DI, not the MonoGame one
        private readonly new IServiceRepository Services;
        private readonly GameTimer Timer;
        private readonly Director Director;

        private static FpsCounter FpsCounter;
        private static Vector2 FpsPosition;
        private static SpriteFont FpsFont;

        public Game1()
        {
            this.Graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            this.Services = new ServiceRepository();
            this.Timer = new GameTimer();
            this.Director = new Director(this.Services);

            FpsCounter = new FpsCounter(this.Timer, 60);
            FpsPosition = new Vector2(10, Graphics.PreferredBackBufferHeight - 22);
        }

        protected override void Initialize()
        {
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);

            var observer = new MonoGameObserver().UseKeyboard().UseMouse();

            this.Services
                .AddSingleton<Game>(this)
                .AddSingleton<IGameTimer>(this.Timer)
                .AddSingleton<Director>(this.Director)
                .AddSingleton<SpriteBatch>(this.SpriteBatch)
                .AddSingleton<ContentManager>(this.Content)
                .AddSingleton<GraphicsDeviceManager>(this.Graphics)
                .AddSingleton<KeyStateObserver>(observer)
                .AddTransient<Ecosystem>()
                .AddTransient<Keycosystem, BasicKeycosystem>()
                .AddTransient<DebugLogger, BasicDebugLogger>()
                .AddTransient<CameraEntity, BasicCamera>()
                .AddTransient<DirectiveRenderer, BasicRenderer>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            FpsFont = this.Content.Load<SpriteFont>("Assets/Debug10");

            this.Director
                .AddScene<GameSelectScene>(Scenes.SceneSelect)
                .AddScene<TestMapScene>(Scenes.TestMapScene)
                .AddScene<DogTreasureHuntScene>(Scenes.DogTreasureHunt)
                .AddScene<KbScene>(Scenes.InputTester)
                .LoadScene(Scenes.SceneSelect);
        }

        protected override void Update(GameTime gameTime)
        {
            this.Timer.Advance();
            this.Director.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.Director.CurrentScene.Draw();
            
            SpriteBatch.Begin();
            SpriteBatch.DrawString(FpsFont, $"FPS: {FpsCounter.Update():0.0}", FpsPosition, Color.White);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public class BasicCamera : CameraEntity
    {
        public BasicCamera(GraphicsDeviceManager gdm) : base(gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight)
        {
        }
    }

    public class BasicRenderer : MonoGameRenderer
    {
        public BasicRenderer(SpriteBatch sb, ContentManager content, CameraEntity camera) : base(sb, content, camera)
        {
            this.BackgroundColor = Color.Black; // todo: make configurable
        }
    }

    public class BasicKeycosystem : Keycosystem
    {
        public BasicKeycosystem(Game game, Director director, IGameTimer timer, KeyStateObserver observer) : base(timer, observer)
        {
            var k = new StandardKeyboard();
            var scheme = new ControlScheme()
                .AddControl(() => k.Escape.IsPressed, game.Exit)
                .AddControl(() => k.F2.DurationPressed > TimeSpan.FromSeconds(2).Ticks, () => director.LoadScene(Scenes.SceneSelect));

            this.AddController(1, k)
                .AddController(1, new StandardMouse())
                .AddControlScheme("System", scheme);
        }
    }

    public class BasicDebugLogger : DebugLogger
    {
        public BasicDebugLogger(ContentManager content, SpriteBatch spriteBatch) : base(spriteBatch)
        {
            this.Font = content.Load<SpriteFont>("Assets/Debug10");
        }
    }
}
