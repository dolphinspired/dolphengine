﻿using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Input.Controllers;
using DolphEngine.MonoGame.Eco.Components;
using DolphEngine.MonoGame.Eco.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DolphEngine.Demo
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Color BackgroundColor = Color.CornflowerBlue;

        private Entity Player;
        private const int moveSpeed = 4;

        private GameTime _currentGameTime;
        private readonly Func<long> GameTimer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Measuring game time in milliseconds until I need something more precise
            this.GameTimer = () => this._currentGameTime.TotalGameTime.Ticks / TimeSpan.TicksPerMillisecond;
        }

        protected override void Initialize()
        {
            Tower.Initialize();
            this.InitializeControls();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Tower.Debug.Font = this.Content.Load<SpriteFont>("Debug");
            var sprite = this.Content.Load<Texture2D>("Assets/link_walk_simple");

            var position = new PositionComponent2d(30, 50);
            var size = new SizeComponent2d(50, 100);
            var anim = AnimatedSpriteComponent.BuildFromSpritesheet(sprite, 8, 4);
            anim.DurationPerFrame = 100;
            anim.Sequence = new List<int> { 24, 25, 26, 27, 28, 29 };

            this.Player = new Entity("Player")
                .AddComponent(anim)
                .AddComponent(position)
                .AddComponent(size);

            Tower.Ecosystem.AddEntity(this.Player);
            Tower.Ecosystem.AddHandler(new SpriteRenderingHandler(this.spriteBatch));
            Tower.Ecosystem.AddHandler(new AnimatedSpriteRenderingHandler(this.spriteBatch, this.GameTimer));
        }

        protected override void Update(GameTime gameTime)
        {
            this._currentGameTime = gameTime;

            this.BackgroundColor = Color.CornflowerBlue; // Reset to default color before reading inputs
            Tower.Keycosystem.Update(this.GameTimer());
            Tower.Ecosystem.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(this.BackgroundColor);

            spriteBatch.Begin(samplerState: SamplerState.PointWrap); // disable anti-aliasing
            Tower.Ecosystem.Draw();
            Tower.Debug.Render(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void InitializeControls()
        {
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

            Tower.Keycosystem
                .AddControlReaction(keyboard, k => k.Down.IsPressed, k => this.Player.GetComponent<PositionComponent2d>().Y += moveSpeed)
                .AddControlReaction(keyboard, k => k.Up.IsPressed, k => this.Player.GetComponent<PositionComponent2d>().Y -= moveSpeed)
                .AddControlReaction(keyboard, k => k.Right.IsPressed, k => this.Player.GetComponent<PositionComponent2d>().X += moveSpeed)
                .AddControlReaction(keyboard, k => k.Left.IsPressed, k => this.Player.GetComponent<PositionComponent2d>().X -= moveSpeed);

            Tower.Debug.AddLine(1,
                () => $"CurrentGameTick: {this.GameTimer()}",
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
        }
    }
}