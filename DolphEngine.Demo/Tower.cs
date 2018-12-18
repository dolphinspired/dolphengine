using DolphEngine.Eco;
using DolphEngine.Input;
using DolphEngine.MonoGame.Input;

namespace DolphEngine.Demo
{
    public static class Tower
    {
        public static void Initialize()
        {
            Debug = new DebugLogger
            {
                Hidden = true,
                CurrentPage = 1
            };

            Ecosystem = new Ecosystem();

            var observer = new MonoGameObserver().UseKeyboard().UseMouse();
            Keycosystem = new Keycosystem(observer);
        }

        public static DebugLogger Debug;

        public static Ecosystem Ecosystem;

        public static Keycosystem Keycosystem;
    }
}
