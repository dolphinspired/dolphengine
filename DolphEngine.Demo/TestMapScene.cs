using DolphEngine.Demo.Entities;
using DolphEngine.Demo.Handlers;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Eco.Entities;
using DolphEngine.Eco.Handlers;
using DolphEngine.Graphics.Sprites;
using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.Input.Controls;
using DolphEngine.MonoGame.Eco.Handlers;
using DolphEngine.MonoGame.Input;
using DolphEngine.Scenery;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DolphEngine.Demo
{
    public class TestMapScene : Scene
    {
        protected readonly ContentManager Content;
        protected readonly SpriteBatch SpriteBatch;
        protected readonly DebugLogger Debug;

        protected PlayerEntity Player;
        protected CameraEntity Camera;

        private readonly StandardKeyboard _keyboard;
        private readonly StandardMouse _mouse;

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

        public TestMapScene(ContentManager content, SpriteBatch spriteBatch, DebugLogger debug, int sceneViewWidth, int sceneViewHeight)
            : base(new Ecosystem(), GetKeycosystem())
        {
            this.Content = content;
            this.SpriteBatch = spriteBatch;
            this.Debug = debug;

            this._sceneViewWidth = sceneViewWidth;
            this._sceneViewHeight = sceneViewHeight;

            this._keyboard = new StandardKeyboard();
            this._mouse = new StandardMouse();
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
            var tileSize = Sprites.Tiles.Frames[0].Size;
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
                        .AddComponent(new SpriteComponent { SpriteSheet = Sprites.Tiles, SpriteSheetIndex = tilevalue })
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

            this.Ecosystem
                .AddEntity(this.Player)
                .AddEntity(this.Camera);

            this.Ecosystem
                .AddHandler<SpeedHandler2d>()
                .AddHandler<SpriteHandler>()
                .AddHandler<TextHandler>()
                .AddHandler(new DrawDirectiveHandler(this.SpriteBatch, this.Content, this.Camera));
        }

        private void LoadControls()
        {
            this.Keycosystem
                .AddControlReaction(this._keyboard, k => k.ArrowKeys.IsPressed, k => {
                    var p = this.Player;
                    var startingAnim = p.Sprite.AnimationSequence;
                    
                    if ((k.ArrowKeys.Direction & Direction.Up) > 0)
                    {
                        p.Speed.X = 2;
                        p.Speed.Y = -1;
                        p.Sprite.AnimationSequence = "WalkNorth";
                    }
                    if ((k.ArrowKeys.Direction & Direction.Right) > 0)
                    {
                        p.Speed.X = 2;
                        p.Speed.Y = 1;
                        p.Sprite.AnimationSequence = "WalkEast";
                    }
                    if ((k.ArrowKeys.Direction & Direction.Down) > 0)
                    {
                        p.Speed.X = -2;
                        p.Speed.Y = 1;
                        p.Sprite.AnimationSequence = "WalkSouth";
                    }
                    if ((k.ArrowKeys.Direction & Direction.Left) > 0)
                    {
                        p.Speed.X = -2;
                        p.Speed.Y = -1;
                        p.Sprite.AnimationSequence = "WalkWest";
                    }

                    if (startingAnim != p.Sprite.AnimationSequence)
                    {
                        var anim = p.Sprite.Animation.GetAnimation(p.Sprite.AnimationSequence);
                        anim.Play(TimeSpan.FromMilliseconds(100), DurationMode.Frame, AnimationReplayMode.Loop);
                    }
                })
                .AddControlReaction(this._keyboard, k => !k.ArrowKeys.IsPressed, k => {
                    var speed = this.Player.Speed;
                    speed.X = 0;
                    speed.Y = 0;
                })
                .AddControlReaction(this._keyboard, k => k.ArrowKeys.JustReleased, k => {
                    var p = this.Player;
                    bool play = false;

                    if (p.Sprite.AnimationSequence == "WalkNorth")
                    {
                        p.Sprite.AnimationSequence = "IdleNorth";
                        play = true;
                    }
                    else if (p.Sprite.AnimationSequence == "WalkEast")
                    {
                        p.Sprite.AnimationSequence = "IdleEast";
                        play = true;
                    }
                    else if (p.Sprite.AnimationSequence == "WalkSouth")
                    {
                        p.Sprite.AnimationSequence = "IdleSouth";
                        play = true;
                    }
                    else if (p.Sprite.AnimationSequence == "WalkWest")
                    {
                        p.Sprite.AnimationSequence = "IdleWest";
                        play = true;
                    }

                    if (play)
                    {
                        var anim = p.Sprite.Animation.GetAnimation(p.Sprite.AnimationSequence);
                        anim.Play(TimeSpan.FromMilliseconds(100), DurationMode.Frame, AnimationReplayMode.Loop);
                    }
                });

            this.Keycosystem
                .AddControlReaction(this._keyboard, k => k.WASD.IsPressed, k =>
                {
                    if ((k.WASD.Direction & Direction.Up) > 0)
                    {
                        this.Camera.Transform.Offset.Y += 8;
                    }
                    if ((k.WASD.Direction & Direction.Right) > 0)
                    {
                        this.Camera.Transform.Offset.X -= 8;
                    }
                    if ((k.WASD.Direction & Direction.Down) > 0)
                    {
                        this.Camera.Transform.Offset.Y -= 8;
                    }
                    if ((k.WASD.Direction & Direction.Left) > 0)
                    {
                        this.Camera.Transform.Offset.X += 8;
                    }
                })
                .AddControlReaction(this._mouse, m => m.Scroll.Y.JustMoved, m =>
                {
                    var zoom = m.Scroll.Y.PositionDelta > 0 ? 0.25f : -0.25f;
                    this.Camera.AdjustZoom(zoom);
                })
                .AddControlReaction(this._mouse, m => m.MiddleClick.JustPressed, m => this.Camera.ResetZoom())
                .AddControlReaction(this._keyboard, k => k.F.JustPressed, k => this.Camera.ResetPan().Focus.Target = this.Player)
                .AddControlReaction(this._keyboard, k => k.G.JustPressed, k => this.Camera.Focus.Target = null);

            this.Debug.AddPage(
                () => "Player info:",
                DebugLogger.EmptyLine,
                () => $"Speed: ({this.Player.Speed.X}, {this.Player.Speed.Y})",
                () => $"X: {this.Player.Position.X}, Y: {this.Player.Position.Y}",
                () => $"Width: {this.Player.Sprite.SpriteSheet.Frames[0].Width}, Height: {this.Player.Sprite.SpriteSheet.Frames[0].Height}",
                DebugLogger.EmptyLine,
                () => "Camera info:",
                DebugLogger.EmptyLine,
                () => $"Position: ({this.Camera.Position.X}, {this.Camera.Position.Y})" +
                $"      Size: ({this.Camera.Size.Width}, {this.Camera.Size.Height})",
                () => $"Offset: ({this.Camera.Transform.Offset.X:0.000}, {this.Camera.Transform.Offset.Y:0.000})" +
                $"      Scale: ({this.Camera.Transform.Scale.X:0.000}, {this.Camera.Transform.Scale.Y:0.000})" +
                $"      Rotation: ({this.Camera.Transform.Rotation:0.000})"
            );
        }

        public void UnloadControls()
        {
            this.Keycosystem
                .RemoveControl(this._keyboard)
                .RemoveControl(this._mouse);
        }
    }
}
