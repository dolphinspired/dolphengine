using DolphEngine.Demo.Games.DogGame;
using DolphEngine.Demo.Games.InputTester;
using DolphEngine.Demo.Games.TestMap;
using DolphEngine.DI;
using DolphEngine.Eco;
using DolphEngine.Graphics;
using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.Messaging;
using DolphEngine.MonoGame;
using DolphEngine.Scenery;
using DolphEngine.UI.Containers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DolphEngine.Demo
{
    public class Game1 : Game
    {
        // Use the DolphEngine implementation of DI, not the MonoGame one
        private readonly new IServiceRepository Services;
        private readonly GraphicsDeviceManager Graphics;
        private readonly GameTimer Timer;
        private Director Director;

        public Game1()
        {
            this.Graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            this.Services = new ServiceRepository();
            this.Timer = new GameTimer();
        }

        protected override void Initialize()
        {
            this.Services
                .AddSingleton<IServiceRepository>(this.Services)
                .AddSingleton<Game>(this)
                .AddSingleton<GameTimer>(this.Timer)
                .AddSingleton<Director>()
                .AddSingleton<SpriteBatch>(new SpriteBatch(this.GraphicsDevice))
                .AddSingleton<ContentManager>(this.Content)
                .AddSingleton<GraphicsDeviceManager>(this.Graphics)
                .AddSingleton<KeyStateObserver>(new MonoGameObserver().UseKeyboard().UseMouse())
                .AddScoped<Ecosystem>()
                .AddScoped<MessageRouter>()
                .AddScoped<Keycosystem, BasicKeycosystem>()
                .AddScoped<DebugLogger, BasicDebugLogger>()
                .AddScoped<DirectiveRenderer, BasicRenderer>()
                .AddScoped<FpsCounter, BasicFpsCounter>()
                .AddScoped<Window, BasicWindow>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.Director = this.Services.GetService<Director>();

            this.Director
                .AddScene<GameSelectScene>(Scenes.SceneSelect)
                .AddScene<TestMapScene>(Scenes.TestMapScene)
                .AddScene<DogTreasureHuntScene>(Scenes.DogTreasureHunt)
                .AddScene<KbScene>(Scenes.InputTester)
                .LoadScene(Scenes.SceneSelect);
        }

        protected override void Update(GameTime gameTime)
        {
            this.Timer.Update();
            this.Director.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.Director.Draw();

            base.Draw(gameTime);
        }
    }

    #region Basic DI implementations

    public class BasicRenderer : MonoGameRenderer
    {
        public BasicRenderer(SpriteBatch sb, ContentManager content, GraphicsDeviceManager gdm) : base(sb, content)
        {
            this.BackgroundColor = Color.Black; // todo: make configurable
            this.AddViewport("default", new Viewport2d(new Rect2d(0, 0, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight, Origin2d.TrueCenter)));
        }
    }

    public class BasicEcosystem : Ecosystem
    {
        public BasicEcosystem(GameTimer timer, DirectiveRenderer renderer) : base(timer, renderer)
        {
            // Put entities common to every scene here, they will be initialized with each scene
        }
    }

    public class BasicKeycosystem : Keycosystem
    {
        public BasicKeycosystem(Game game, Director director, GameTimer timer, KeyStateObserver observer) : base(timer, observer)
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

    public class BasicFpsCounter : FpsCounter
    {
        public BasicFpsCounter(GraphicsDeviceManager gdm, ContentManager content, SpriteBatch sb, GameTimer timer) : base(sb, timer)
        {
            this.Font = content.Load<SpriteFont>("Assets/Debug10");
            this.Position = new Vector2d(10, gdm.PreferredBackBufferHeight - 22);
            this.SetSampleSize(60);
        }
    }

    public class BasicWindow : Window
    {
        public BasicWindow(GraphicsDeviceManager gdm)
        {
            this.Space = new Rect2d(0, 0, gdm.PreferredBackBufferWidth, gdm.PreferredBackBufferHeight);
        }
    }

    #endregion
}
