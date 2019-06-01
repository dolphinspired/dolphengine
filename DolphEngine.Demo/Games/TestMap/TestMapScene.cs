using DolphEngine.Demo.Games.TestMap.Entities;
using DolphEngine.Demo.Games.TestMap.Handlers;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Eco.Entities;
using DolphEngine.Eco.Handlers;
using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.MonoGame;
using DolphEngine.Scenery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DolphEngine.Demo.Games.TestMap
{
    public class TestMapScene : IScene
    {
        protected readonly Ecosystem Ecosystem;
        protected readonly Keycosystem Keycosystem;
        protected readonly DebugLogger DebugLogger;
        protected readonly Director Director;

        protected readonly ContentManager Content;
        protected readonly SpriteBatch SpriteBatch;

        protected PlayerEntity Player;
        protected CameraEntity Camera;

        private readonly int _sceneViewWidth;
        private readonly int _sceneViewHeight;

        private readonly int[][] TestBoard = new int[][]
        {
            new int[] { 0, 0, 0, 2, 0, 1 },
            new int[] { 0, 0, 0, 2, 0, 1 },
            new int[] { 3, 2, 2, 2, 0, 1 },
            new int[] { 3, 2, 0, 0, 0, 1 },
            new int[] { 0, 2, 0, 3, 3, 1 },
            new int[] { 0, 2, 0, 3, 3, 1 },
        };

        public TestMapScene(
            Ecosystem ecosystem, 
            Keycosystem keycosystem, 
            DebugLogger debugLogger,
            SpriteBatch spriteBatch,
            ContentManager content,
            GraphicsDeviceManager gdm,
            Director director)
        {
            this.Ecosystem = ecosystem;
            this.Keycosystem = keycosystem;
            this.DebugLogger = debugLogger;

            this.Content = content;
            this.SpriteBatch = spriteBatch;

            this._sceneViewWidth = gdm.PreferredBackBufferWidth;
            this._sceneViewHeight = gdm.PreferredBackBufferHeight;
        }

        public void Load()
        {
            this.LoadMap();
            this.LoadEntities();
            this.LoadControls();
        }

        public void Unload()
        {
            this.UnloadControls();
        }

        public void Update()
        {
            this.Ecosystem.Update();
            this.Keycosystem.Update();
        }

        public void Draw()
        {
            this.Ecosystem.Draw();
            this.DebugLogger.Render();
        }

        private void LoadMap()
        {
            var tileSize = Sprites.Tiles.Frames[0].Size;
            var start = Position2d.Zero;
            var origin = new Origin2d(Anchor2d.TopCenter);

            int xShift = 32;
            int yShift = 16;

            var i = 0;
            var row = 0;
            foreach (var tilerow in this.TestBoard)
            {
                var row_x = start.X - row * xShift;
                var row_y = start.Y + row * yShift;

                var col = 0;
                foreach (var tilevalue in tilerow)
                {
                    var x = row_x + col * xShift;
                    var y = row_y + col * yShift;

                    var tileEntity = new Entity(new Rect2d(x, y, tileSize.Width, tileSize.Height, origin), $"Tile_{i++}");
                    tileEntity.AddComponent(new SpriteComponent { SpriteSheet = Sprites.Tiles, Index = tilevalue });
                    tileEntity.AddComponent<DrawComponent>();

                    this.Ecosystem.AddEntity(tileEntity);

                    col++;
                }

                row++;
            }
        }

        private void LoadEntities()
        {
            this.Player = new PlayerEntity();
            this.Player.Text.Text = "Alphonse";
            this.Player.Text.FontAssetName = "Debug";
            this.Player.Sprite.EnableBoxOutline = true;

            this.Camera = new CameraEntity(this._sceneViewWidth, this._sceneViewHeight);

            var arrow1 = new GlyphEntity(0, "Arrow");
            arrow1.Space = new Rect2d(0, 0, 21, 11, new Origin2d(Anchor2d.MiddleRight));
            arrow1.AddComponent(new LinkedPositionComponent(this.Player, e => e.Space.GetAnchorPosition(Anchor2d.MiddleLeft).Shift(new Vector2d(-5, 0))));
            arrow1.Sprite.Scale = new Vector2d(2, 2);
            arrow1.Sprite.OffsetAnimation = Animations.Select(TimeSpan.FromSeconds(1));
            arrow1.Sprite.EnableBoxOutline = true;

            var ball1 = new GlyphEntity(2, "Ball (rotating)");
            ball1.Space = new Rect2d(100, 50, 11, 11, Origin2d.TrueCenter);
            ball1.Sprite.Scale = new Vector2d(4, 4);
            ball1.Sprite.RotationAnimation = Animations.Rotate(TimeSpan.FromSeconds(1));
            ball1.Sprite.EnableBoxOutline = true;

            var ball2 = new GlyphEntity(2, "Ball (breathing)");
            ball2.Space = new Rect2d(100, 150, 11, 11, Origin2d.TrueCenter);
            ball2.Sprite.Scale = new Vector2d(4, 4);
            ball2.Sprite.ScaleAnimation = Animations.Breathe(TimeSpan.FromSeconds(4));
            ball2.Sprite.EnableBoxOutline = true;

            var shape = new Entity("TestPolygon")
                .AddComponent(new PolygonComponent
                {
                    Color = 0xFF0000FF,
                    Polygon = new Polygon2d(
                        // This should make a "Z" shape
                        new Vector2d(100, 0),
                        new Vector2d(-100, 100),
                        new Vector2d(100, 0)
                    )
                })
                .AddComponent<DrawComponent>();
            shape.Space.Position.Shift(-300, -150);

            this.Ecosystem.AddEntities(
                this.Player,
                this.Camera,
                arrow1,
                ball1,
                ball2,
                shape);

            this.Ecosystem
                .AddHandler<SpeedHandler>()
                .AddHandler<LinkedPositionHandler>()
                .AddHandler<SpriteStateHandler>()
                .AddHandler<TextHandler>()
                .AddHandler<SpriteHandler>()
                .AddHandler<PolygonHandler>()
                .AddHandler(new DrawHandler(new MonoGameRenderer(this.SpriteBatch, this.Content, this.Camera)));

            this.DebugLogger.AddPage(
                () => Camera.ToString(),
                () => Player.ToString(),
                () => arrow1.ToString(),
                () => ball1.ToString(),
                () => ball2.ToString());
        }

        private void LoadControls()
        {
            var k = this.Keycosystem.GetController<StandardKeyboard>(1);
            var m = this.Keycosystem.GetController<StandardMouse>(1);

            this.Keycosystem.AddControlScheme("DebugNav", ControlSchemes.DebugNavigation(this.DebugLogger, k));

            var context = new ControlScheme()
                .AddControl(() => k.ArrowKeys.IsPressed, () => {
                    var p = this.Player;
                    
                    if ((k.ArrowKeys.Direction & Direction2d.Up) > 0)
                    {
                        p.Speed.X = 2;
                        p.Speed.Y = -1;
                        p.Facing.Direction = Direction2d.Up;
                    }
                    if ((k.ArrowKeys.Direction & Direction2d.Right) > 0)
                    {
                        p.Speed.X = 2;
                        p.Speed.Y = 1;
                        p.Facing.Direction = Direction2d.Right;
                    }
                    if ((k.ArrowKeys.Direction & Direction2d.Down) > 0)
                    {
                        p.Speed.X = -2;
                        p.Speed.Y = 1;
                        p.Facing.Direction = Direction2d.Down;
                    }
                    if ((k.ArrowKeys.Direction & Direction2d.Left) > 0)
                    {
                        p.Speed.X = -2;
                        p.Speed.Y = -1;
                        p.Facing.Direction = Direction2d.Left;
                    }
                })
                .AddControl(() => !k.ArrowKeys.IsPressed, () => {
                    var speed = this.Player.Speed;
                    speed.X = 0;
                    speed.Y = 0;
                })
                .AddControl(() => k.WASD.IsPressed, () =>
                {
                    if ((k.WASD.Direction & Direction2d.Up) > 0)
                    {
                        this.Camera.Space.Position.Y -= 8;
                    }
                    if ((k.WASD.Direction & Direction2d.Right) > 0)
                    {
                        this.Camera.Space.Position.X += 8;
                    }
                    if ((k.WASD.Direction & Direction2d.Down) > 0)
                    {
                        this.Camera.Space.Position.Y += 8;
                    }
                    if ((k.WASD.Direction & Direction2d.Left) > 0)
                    {
                        this.Camera.Space.Position.X -= 8;
                    }
                })
                .AddControl(() => m.Scroll.Y.JustMoved, () =>
                {
                    var zoom = m.Scroll.Y.PositionDelta > 0 ? 0.25f : -0.25f;
                    this.Camera.Lens.Zoom += zoom;
                })
                .AddControl(() => m.MiddleClick.JustPressed, () => this.Camera.Lens.Zoom = 1.000f)
                .AddControl(() => k.F.JustPressed, () => this.Camera.Lens.Focus = this.Player)
                .AddControl(() => k.G.JustPressed, () => this.Camera.Lens.Focus = null)
                .AddControl(() => k.LeftShift.DurationPressed > TimeSpan.FromSeconds(1).Ticks && k.Z.DurationPressed > TimeSpan.FromSeconds(1).Ticks, () =>
                {
                    this.Director.LoadScene(Scenes.SceneSelect);
                });

            var pauseContext = new ControlScheme();

            pauseContext
                .AddControl(() => k.P.JustPressed, () =>
                {
                    context.Enabled = !context.Enabled;
                });
            
            this.Keycosystem
                .AddControlScheme("TestMapScene", context)
                .AddControlScheme("Paused", pauseContext);
        }

        public void UnloadControls()
        {
            this.Keycosystem
                .RemoveControlScheme("DebugNav")
                .RemoveControlScheme("TestMapScene")
                .RemoveControlScheme("Paused");
        }
    }
}
