using DolphEngine.Demo.Entities;
using DolphEngine.Demo.Handlers;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Eco.Entities;
using DolphEngine.Eco.Handlers;
using DolphEngine.Input;
using DolphEngine.MonoGame.Graphics;
using DolphEngine.MonoGame.Input;
using DolphEngine.Scenery;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace DolphEngine.Demo
{
    public class TestMapScene : Scene
    {
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

        public TestMapScene(ContentManager content, SpriteBatch spriteBatch, int sceneViewWidth, int sceneViewHeight)
            : base(new Ecosystem(Tower.Timer), GetKeycosystem())
        {
            this.Content = content;
            this.SpriteBatch = spriteBatch;

            this._sceneViewWidth = sceneViewWidth;
            this._sceneViewHeight = sceneViewHeight;
        }

        private static Keycosystem GetKeycosystem()
        {
            var observer = new MonoGameObserver().UseKeyboard().UseMouse();
            return new Keycosystem(observer);
        }

        public override void Load()
        {
            this.LoadMap();
            this.LoadEntities();
            this.LoadControls();
        }

        public override void Unload()
        {
            this.UnloadControls();
        }

        private void LoadMap()
        {
            var tileSize = Sprites.Tiles.Frames[0].GetSize();
            var start = new Position2d(200, 20);

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

                    this.Ecosystem
                        .AddEntity(new Entity($"Tile_{i++}")
                        .AddComponent(new PositionComponent2d(x, y))
                        .AddComponent(new SpriteComponent { SpriteSheet = Sprites.Tiles, StaticSprite = tilevalue })
                        .AddComponent<DrawComponent>());

                    col++;
                }

                row++;
            }
        }

        private void LoadEntities()
        {
            this.Player = new PlayerEntity();
            this.Player.Position.Set(30, 50);
            this.Player.Text.Text = "Alphonse";
            this.Player.Text.FontAssetName = "Debug";

            this.Camera = new CameraEntity(this._sceneViewWidth, this._sceneViewHeight);
            this.Camera.Pan(240, 120);

            var arrow1 = new GlyphEntity(0);
            arrow1.AddComponent(new LinkedPositionComponent2d(this.Player));
            arrow1.Sprite.StaticTransform = new Transform2d(-50, 30, 2, 2, 0);

            var ball1 = new GlyphEntity(2);
            ball1.Position.Set(400, 100);
            ball1.Sprite.StaticTransform = new Transform2d(0, 0, 4, 4, 0);
            ball1.Sprite.AnimationSet = Animations.Glyph;
            ball1.Sprite.AnimatedTransform = "Rotate";

            var ball2 = new GlyphEntity(2);
            ball2.Position.Set(400, 200);
            ball2.Sprite.StaticTransform = new Transform2d(0, 0, 4, 4, 0);
            ball2.Sprite.AnimationSet = Animations.Glyph;
            ball2.Sprite.AnimatedTransform = "Breathe";

            this.Ecosystem.AddEntities(
                this.Player,
                this.Camera,
                arrow1,
                ball1,
                ball2);

            this.Ecosystem
                .AddHandler<SpeedHandler>()
                .AddHandler<LinkedPositionHandler>()
                .AddHandler<SpriteStateHandler>()
                .AddHandler<TextHandler>()
                .AddHandler<SpriteHandler>()
                .AddHandler(new DrawHandler(new MonoGameRenderer(this.SpriteBatch, this.Content, this.Camera)));
        }

        private void LoadControls()
        {
            var context = new KeyContext("TestMapScene")
                .AddControl(Tower.Keyboard, k => k.ArrowKeys.IsPressed, k => {
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
                .AddControl(Tower.Keyboard, k => !k.ArrowKeys.IsPressed, k => {
                    var speed = this.Player.Speed;
                    speed.X = 0;
                    speed.Y = 0;
                })
                .AddControl(Tower.Keyboard, k => k.WASD.IsPressed, k =>
                {
                    if ((k.WASD.Direction & Direction2d.Up) > 0)
                    {
                        this.Camera.Transform.Offset.Y += 8;
                    }
                    if ((k.WASD.Direction & Direction2d.Right) > 0)
                    {
                        this.Camera.Transform.Offset.X -= 8;
                    }
                    if ((k.WASD.Direction & Direction2d.Down) > 0)
                    {
                        this.Camera.Transform.Offset.Y -= 8;
                    }
                    if ((k.WASD.Direction & Direction2d.Left) > 0)
                    {
                        this.Camera.Transform.Offset.X += 8;
                    }
                })
                .AddControl(Tower.Mouse, m => m.Scroll.Y.JustMoved, m =>
                {
                    var zoom = m.Scroll.Y.PositionDelta > 0 ? 0.25f : -0.25f;
                    this.Camera.AdjustZoom(zoom);
                })
                .AddControl(Tower.Mouse, m => m.MiddleClick.JustPressed, m => this.Camera.ResetZoom())
                .AddControl(Tower.Keyboard, k => k.F.JustPressed, k => this.Camera.ResetPan().Focus.Target = this.Player)
                .AddControl(Tower.Keyboard, k => k.G.JustPressed, k => this.Camera.Focus.Target = null);

            var pauseContext = new KeyContext("Paused");

            pauseContext
                .AddControl(Tower.Keyboard, k => k.P.JustPressed, p =>
                {
                    context.Enabled = !context.Enabled;
                });
            
            this.Keycosystem
                .AddContext(context)
                .AddContext(pauseContext);

            Tower.DebugLogger.AddPage(
                () => "Player info:",
                DebugLogger.EmptyLine,
                () => $"Speed: ({this.Player.Speed.X}, {this.Player.Speed.Y})",
                () => $"X: {this.Player.Position.X}, Y: {this.Player.Position.Y}",
                DebugLogger.EmptyLine,
                () => "Camera info:",
                DebugLogger.EmptyLine,
                () => $"Position: ({this.Camera.Position.X}, {this.Camera.Position.Y})" +
                $"      Size: ({this.Camera.Size.Width}, {this.Camera.Size.Height})",
                () => $"Offset: ({this.Camera.Transform.Offset.X:0.000}, {this.Camera.Transform.Offset.Y:0.000})" +
                $"      Scale: ({this.Camera.Transform.Scale.X:0.000}, {this.Camera.Transform.Scale.Y:0.000})" +
                $"      Rotation: ({this.Camera.Transform.Rotation:0.000})"
            );

            Tower.DebugLogger.AddPage(
                () => $"Entities: {string.Join(',', this.Ecosystem.GetEntities().Where(x => !x.Name.StartsWith("Tile")))}");
        }

        public void UnloadControls()
        {
            this.Keycosystem
                .RemoveContext("TestMapScene")
                .RemoveContext("Paused");
        }
    }
}
