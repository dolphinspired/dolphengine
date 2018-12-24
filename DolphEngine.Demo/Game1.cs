using DolphEngine.Demo.Components;
using DolphEngine.Demo.Handlers;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Input.Controllers;
using DolphEngine.MonoGame;
using DolphEngine.MonoGame.Eco.Components;
using DolphEngine.MonoGame.Eco.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Tasks;

namespace DolphEngine.Demo
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Entity Player;
        private const int MoveSpeed = 4;

        private GameTime _currentGameTime;
        private readonly Func<long> GameTimer;

        private readonly int[][] TestMap = new int[][]
        {
            new int[] { 0, 0, 0, 2, 0, 1 },
            new int[] { 0, 0, 0, 2, 0, 1 },
            new int[] { 3, 2, 2, 2, 0, 1 },
            new int[] { 3, 2, 0, 0, 0, 1 },
            new int[] { 0, 2, 0, 3, 3, 1 },
            new int[] { 0, 2, 0, 3, 3, 1 },
        };

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
            Tower.Content = this.Content;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // todo: experimental. Making this async caused a controller delegate to run early, before this.Player was set. Even though
            // no keys were pressed?

            //await Task.WhenAll
            //(
            //    Tower.Content.LoadAllAsync<SpriteFont>("Debug"),
            //    Tower.Content.LoadAllAsync<Texture2D>("Assets/link_walk_simple", "Assets/iso_tiles_32_single")
            //);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Tower.Debug.Font = this.Content.Load<SpriteFont>("Debug");

            var position = new PositionComponent2d(30, 50);
            var tset = Tileset.FromSpritesheet(this.Content.Load<Texture2D>("Assets/Alphonse"), 12, 8);
            var anim = new AnimatedSpriteComponent { Tileset = tset };
            anim.DurationPerFrame = 100;

            this.Player = new Entity("Player")
                .AddComponent<PlayerComponent>()
                .AddComponent(anim)
                .AddComponent(position);

            this.LoadMap();

            Tower.Ecosystem.AddEntity(this.Player);
            Tower.Ecosystem.AddHandler(new SpriteRenderingHandler(this.spriteBatch));
            Tower.Ecosystem.AddHandler(new SpritesheetRenderingHandler(this.spriteBatch));
            Tower.Ecosystem.AddHandler(new AnimatedSpriteRenderingHandler(this.spriteBatch, this.GameTimer));
            Tower.Ecosystem.AddHandler<PlayerHandler>();
        }

        protected override void Update(GameTime gameTime)
        {
            this._currentGameTime = gameTime;
            
            Tower.Keycosystem.Update(this.GameTimer());
            Tower.Ecosystem.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

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
                .AddControlReaction(keyboard, k => k.ArrowKeys.IsPressed, k => {
                    var pc = this.Player.GetComponent<PlayerComponent>();
                    pc.Speed = MoveSpeed;
                    pc.Direction = k.ArrowKeys.Direction;
                })
                .AddControlReaction(keyboard, k => !k.ArrowKeys.IsPressed, k => {
                    this.Player.GetComponent<PlayerComponent>().Speed = 0;
                });
            
            Tower.Debug.AddLine(1,
                () => "Player info:",
                DebugLogger.EmptyLine,
                () => $"Speed: {this.Player.GetComponent<PlayerComponent>().Speed}, Direction: {this.Player.GetComponent<PlayerComponent>().Direction}",
                () => $"X: {this.Player.GetComponent<PositionComponent2d>().X}, Y: {this.Player.GetComponent<PositionComponent2d>().Y}",
                () => $"Width: {this.Player.GetComponent<AnimatedSpriteComponent>().SourceRect?.Width}, Height: {this.Player.GetComponent<AnimatedSpriteComponent>().SourceRect?.Height}");

            Tower.Debug.AddLine(2,
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
                () => $"X: {mouse.Cursor.X}, Y: {mouse.Cursor.Y}");

            Tower.Debug.AddLine(2, "This is page 2!");
        }

        private void LoadMap()
        {
            var tileset = Tileset.FromSpritesheet(this.Content.Load<Texture2D>("Assets/iso_tiles_32_single"), 4, 4);
            var start = new Position2d(200, 20);

            var i = 0;
            var row = 0;
            foreach (var tilerow in this.TestMap)
            {
                var row_x = start.X - row * 30;
                var row_y = start.Y + row * 15;

                var col = 0;
                foreach (var tilevalue in tilerow)
                {
                    var x = row_x + col * 30;
                    var y = row_y + col * 15;

                    var sprite = new SpritesheetComponent
                    {
                        Tileset = tileset,
                        CurrentFrame = tilevalue
                    };

                    Tower.Ecosystem.AddEntity(new Entity($"Tile_{i++}")
                        .AddComponent(sprite)
                        .AddComponent(new PositionComponent2d(x, y)));

                    col++;
                }

                row++;
            }
        }
    }
}
