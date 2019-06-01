using DolphEngine.Demo.Games.DogGame;
using DolphEngine.Demo.Games.TestMap;
using DolphEngine.DI;
using DolphEngine.Eco;
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

        private readonly StandardKeyboard Keyboard = new StandardKeyboard();
        private readonly StandardMouse Mouse = new StandardMouse();

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
                .AddSingleton<IGameTimer>(this.Timer)
                .AddSingleton<Director>(this.Director)
                .AddSingleton<SpriteBatch>(this.SpriteBatch)
                .AddSingleton<ContentManager>(this.Content)
                .AddSingleton<GraphicsDeviceManager>(this.Graphics)
                .AddSingleton<KeyStateObserver>(observer)
                .AddTransient<Ecosystem>()
                .AddSingletonWithInit<Keycosystem>(keycosystem =>
                {
                    keycosystem
                        .AddController(1, new StandardKeyboard())
                        .AddController(1, new StandardMouse())
                        .AddControlScheme("System", ControlSchemes.System(this, this.Keyboard));
                })
                .AddSingletonWithInit<DebugLogger>(dl =>
                {
                    dl.Font = this.Content.Load<SpriteFont>("Debug");
                });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            FpsFont = this.Content.Load<SpriteFont>("Debug");

            this.Director
                .AddScene<GameSelectScene>(Scenes.SceneSelect)
                .AddScene<TestMapScene>(Scenes.TestMapScene)
                .AddScene<DogTreasureHuntScene>(Scenes.DogTreasureHunt)
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
}
