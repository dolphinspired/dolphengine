using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TacticsGame.Engine;

namespace TacticsGame
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly List<ISpritable> _drawables = new List<ISpritable>();

        public Game1()
        {

            this._graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            SpriteBatchManager.Initialize(this.GraphicsDevice);

            var s1 = new StaticSprite("tiles", new AtlasInfo(4, 4), 0);
            var s2 = new AnimatedSprite("Alphonse", new AtlasInfo(12, 8), AnimationSchedules.WalkCycle);

            s1.Dest = new Point(100, 100);
            s2.Dest = new Point(300, 200);

            this._drawables.AddRange(new ISpritable[] { s1, s2 });

            base.Initialize();
        }

        protected override void LoadContent()
        {
            foreach (var drawable in this._drawables)
            {
                drawable.Load(this.Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GameContext.Frame++;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatchManager.Basic.Begin();
            foreach (var drawable in this._drawables)
            {
                drawable.Draw(gameTime, SpriteBatchManager.Basic);
            }
            SpriteBatchManager.Basic.End();

            base.Draw(gameTime);
        }
    }
}
