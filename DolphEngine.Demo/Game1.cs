﻿using DolphEngine.Demo.Games.DogGame;
using DolphEngine.Demo.Games.TestMap;
using DolphEngine.DI;
using DolphEngine.Eco;
using DolphEngine.Input;
using DolphEngine.MonoGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.Demo
{
    public class Game1 : Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;

        // Use the DolphEngine implementation of DI, not the MonoGame one
        private readonly new IServiceRepository Services = new ServiceRepository();

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
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);

            Tower.Initialize();

            Tower.Keycosystem
                .AddContext(ControlContexts.System(this))
                .AddContext(ControlContexts.DebugNavigation());

            Tower.DebugLogger
                .AddControlInfo(ControlContexts.Keyboard, ControlContexts.Mouse);

            Tower.Director
                .AddService(Tower.Timer)
                .AddService(() => new Ecosystem(Tower.Timer))
                .AddService(() => new Keycosystem(new MonoGameObserver().UseKeyboard().UseMouse()))
                .AddService(this.Content)
                .AddService(this.SpriteBatch)
                .AddService(this.Graphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Tower.DebugLogger.Font = this.Content.Load<SpriteFont>("Debug");

            Tower.Director
                .AddScene<GameSelectScene>(Scenes.SceneSelect)
                .AddScene<TestMapScene>(Scenes.TestMapScene)
                .AddScene<DogTreasureHuntScene>(Scenes.DogTreasureHunt)
                .LoadScene(Scenes.SceneSelect);
        }

        protected override void Update(GameTime gameTime)
        {
            Tower.Timer.Advance();
            
            Tower.Keycosystem.Update(gameTime.TotalGameTime);
            Tower.Director.Update();

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
