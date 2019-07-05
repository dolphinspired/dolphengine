using DolphEngine.Demo.Games.TestMap.Entities;
using DolphEngine.Demo.Games.TestMap.Handlers;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Eco.Handlers;
using DolphEngine.Graphics;
using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.Messaging;
using DolphEngine.Scenery;
using System;
using System.Collections.Generic;

namespace DolphEngine.Demo.Games.TestMap
{
    public class TestMapScene : IScene
    {
        protected readonly Ecosystem Ecosystem;
        protected readonly Keycosystem Keycosystem;
        protected readonly DebugLogger DebugLogger;
        protected readonly Director Director;
        protected readonly DirectiveRenderer Renderer;
        protected readonly FpsCounter FpsCounter;
        protected readonly MessageRouter MessageRouter;
        protected readonly GameTimer Timer;

        protected PlayerEntity Player;
        protected Viewport2d Camera;
        protected PubKey<Position2d> PlayerPosition;
        protected SubKey SubKey;

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
            Director director,
            DirectiveRenderer renderer,
            FpsCounter fpsCounter,
            MessageRouter messageRouter,
            GameTimer timer)
        {
            this.Ecosystem = ecosystem;
            this.Keycosystem = keycosystem;
            this.DebugLogger = debugLogger;
            this.Renderer = renderer;
            this.FpsCounter = fpsCounter;
            this.MessageRouter = messageRouter;
            this.Timer = timer;

            this.PlayerPosition = messageRouter.GetPubKey<Position2d>("player-position");
            this.SubKey = messageRouter.GetSubKey();
            this.Camera = renderer.GetViewport("default");
        }

        public void Load()
        {
            this.LoadMap();
            this.LoadEntities();
            this.LoadControls();

            this.Renderer.AddViewChannel("default", this.Ecosystem);
        }

        public void Unload()
        {
            
        }

        public void Update()
        {
            this.Ecosystem.Update();
            this.Keycosystem.Update();

            if (this.Timer.Frames % 10 == 0)
            {
                this.PlayerPosition.Publish(this.Player.Space.GetOriginPosition());
            }

            this.MessageRouter.Update();
            this.FpsCounter.Update();
        }

        public void Draw()
        {
            this.Renderer.Draw();
            this.DebugLogger.Draw();
            this.FpsCounter.Draw();
        }

        private void LoadMap()
        {
            var tileSize = Sprites.Tiles.Frames[0].GetSize();
            var start = Position2d.Zero;
            var origin = new Origin2d(Anchor2d.TopCenter);

            int xShift = 32;
            int yShift = 16;

            var i = 0;
            var row = 0;
            var entities = new List<Entity>();

            foreach (var tilerow in this.TestBoard)
            {
                var row_x = start.X - row * xShift;
                var row_y = start.Y + row * yShift;

                var col = 0;
                foreach (var tilevalue in tilerow)
                {
                    var x = row_x + col * xShift;
                    var y = row_y + col * yShift;

                    var tileEntity = new Entity(new Rect2d(x, y, tileSize.Width, tileSize.Height, origin));
                    tileEntity.AddComponent(new SpriteComponent { SpriteSheet = Sprites.Tiles, Index = tilevalue });
                    entities.Add(tileEntity);

                    col++;
                }

                row++;
            }

            this.Ecosystem.AddEntities(n => $"Tile_{n}", entities);
        }

        private void LoadEntities()
        {
            this.Player = new PlayerEntity();
            this.Player.Text.Text = "Alphonse";
            this.Player.Text.FontAssetName = "Assets/Debug10";
            this.Player.Text.Color = new ColorRGBA(0, 255, 255);
            this.Player.Sprite.EnableBoxOutline = true;
            this.Ecosystem.AddEntity("Player", this.Player);

            var arrow1 = new GlyphEntity(0);
            arrow1.Space = new Rect2d(0, 0, 21, 11, new Origin2d(Anchor2d.MiddleRight));
            arrow1.AddComponent(new LinkedPositionComponent(this.Player, e => e.Space.GetAnchorPosition(Anchor2d.MiddleLeft).Shift(new Vector2d(-5, 0))));
            arrow1.Sprite.Scale = new Vector2d(2, 2);
            arrow1.Sprite.OffsetAnimation = Animations.Select(TimeSpan.FromSeconds(1));
            arrow1.Sprite.EnableBoxOutline = true;
            this.Ecosystem.AddEntity("Arrow", arrow1);

            var ball1 = new GlyphEntity(2);
            ball1.Space = new Rect2d(100, 50, 11, 11, Origin2d.TrueCenter);
            ball1.Sprite.Scale = new Vector2d(4, 4);
            ball1.Sprite.RotationAnimation = Animations.Rotate(TimeSpan.FromSeconds(1));
            ball1.Sprite.EnableBoxOutline = true;
            this.Ecosystem.AddEntity("Ball (rotating)", ball1);

            var ball2 = new GlyphEntity(2);
            ball2.Space = new Rect2d(100, 150, 11, 11, Origin2d.TrueCenter);
            ball2.Sprite.Scale = new Vector2d(4, 4);
            ball2.Sprite.ScaleAnimation = Animations.Breathe(TimeSpan.FromSeconds(4));
            ball2.Sprite.EnableBoxOutline = true;
            this.Ecosystem.AddEntity("Ball (breathing)", ball2);

            var shape = new Entity()
                .AddComponent(new PolygonComponent
                {
                    Color = new ColorRGBA(255, 0, 0),
                    Polygon = new Polygon2d(
                        // This should make a "Z" shape
                        new Vector2d(100, 0),
                        new Vector2d(-100, 100),
                        new Vector2d(100, 0)
                    )
                });
            shape.Space.Shift(-300, -150);
            this.Ecosystem.AddEntity("TestPolygon", shape);

            this.SubKey.Subscribe<Position2d>("player-position", position =>
            {
                var polygon = shape.GetComponent<PolygonComponent>();
                if (position.Y < 0)
                {
                    polygon.Color = new ColorRGBA(255, 255, 0);
                }
                else
                {
                    polygon.Color = new ColorRGBA(255, 0, 0);
                }
            });

            this.Ecosystem
                .AddHandler<SpeedHandler>()
                .AddHandler<LinkedPositionHandler>()
                .AddHandler<SpriteStateHandler>()
                .AddHandler<TextHandler>()
                .AddHandler<SpriteHandler>()
                .AddHandler<PolygonHandler>();

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
                .AddControl(() => k.WASD.IsPressed, () => ControlSchemes.PanCamera(this.Camera, k.WASD))
                .AddControl(() => m.Scroll.Y.JustMoved, () =>
                {
                    var zoom = m.Scroll.Y.PositionDelta > 0 ? 0.25f : -0.25f;
                    this.Camera.Zoom += zoom;
                })
                .AddControl(() => m.MiddleClick.JustPressed, () => this.Camera.Zoom = 1.000f)
                .AddControl(() => k.F.JustPressed, () => this.Camera.Focus = () => this.Player.Space.GetOriginPosition())
                .AddControl(() => k.G.JustPressed, () => this.Camera.Focus = null)
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
    }
}
