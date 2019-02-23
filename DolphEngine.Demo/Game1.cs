﻿using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.MonoGame.Input;
using DolphEngine.Scenery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DolphEngine.Demo
{
    public class Game1 : Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;

        private GameTime _currentGameTime;
        private readonly Func<long> GameTimer;
        
        private static Vector2 FpsPosition;

        protected readonly Director Director;
        protected readonly DebugLogger Debug;
        protected readonly Keycosystem GlobalKeycosystem;

        public Game1()
        {
            this.Graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            // Measuring game time in milliseconds until I need something more precise
            this.GameTimer = () => this._currentGameTime.TotalGameTime.Ticks / TimeSpan.TicksPerMillisecond;

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
            this._currentGameTime = gameTime;
            
            this.GlobalKeycosystem.Update(this.GameTimer());
            this.Director.CurrentScene.Update(gameTime.TotalGameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.Director.CurrentScene.Draw(gameTime.TotalGameTime);
            this.Debug.Render(SpriteBatch);
            
            SpriteBatch.Begin();
            SpriteBatch.DrawString(this.Debug.Font, $"FPS: {1 / gameTime.ElapsedGameTime.TotalSeconds:0.0}", FpsPosition, this.Debug.FontColor);
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
                () => $"CurrentGameTick: {this.GameTimer()}",
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
