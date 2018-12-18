using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Input.Controllers;
using DolphEngine.MonoGame.Eco.Components;
using DolphEngine.MonoGame.Eco.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DolphEngine.Demo
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Color BackgroundColor = Color.CornflowerBlue;

        private long CurrentTick;

        private PositionComponent2d RectPosition = new PositionComponent2d(30, 50);
        private SizeComponent2d RectSize = new SizeComponent2d(50, 100);
        private Color RectColor = Color.Red;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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

            var data = new Color[this.RectSize.Width * this.RectSize.Height];
            var texture = new Texture2D(this.GraphicsDevice, this.RectSize.Width, this.RectSize.Height);

            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = new Color(i / 10, i / 10, i / 10);
            }

            texture.SetData(data);

            var player = new Entity("Player")
                .AddComponent(new SpriteComponent2d { Texture = texture })
                .AddComponent(this.RectPosition)
                .AddComponent(this.RectSize);

            Tower.Ecosystem.AddEntity(player);
            Tower.Ecosystem.AddHandler(new SpriteRenderingHandler(this.spriteBatch));
        }

        protected override void Update(GameTime gameTime)
        {
            this.CurrentTick = gameTime.TotalGameTime.Ticks;

            this.BackgroundColor = Color.CornflowerBlue; // Reset to default color before reading inputs
            Tower.Keycosystem.Update(gameTime.TotalGameTime.Ticks);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(this.BackgroundColor);

            spriteBatch.Begin(samplerState: SamplerState.PointWrap); // disable anti-aliasing
            Tower.Ecosystem.RunAllHandlers();
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
                .AddControlReaction(keyboard, k => k.Down.IsPressed, k => this.RectPosition.Y++)
                .AddControlReaction(keyboard, k => k.Up.IsPressed, k => this.RectPosition.Y--)
                .AddControlReaction(keyboard, k => k.Right.IsPressed, k => this.RectPosition.X++)
                .AddControlReaction(keyboard, k => k.Left.IsPressed, k => this.RectPosition.X--);

            Tower.Debug.AddLine(1,
                () => $"CurrentGameTick: {CurrentTick}",
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
