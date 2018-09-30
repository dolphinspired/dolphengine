using Microsoft.Xna.Framework.Graphics;

namespace TacticsGame.Engine
{
    public static class SpriteBatchManager
    {
        public static SpriteBatch Basic => _basic;
        private static SpriteBatch _basic;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _basic = new SpriteBatch(graphicsDevice);
        }
    }
}