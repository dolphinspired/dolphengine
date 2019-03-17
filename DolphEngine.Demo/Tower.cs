using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.MonoGame.Input;
using DolphEngine.Scenery;

namespace DolphEngine.Demo
{
    public static class Tower
    {
        public static void Initialize()
        {
            Timer = new GameTimer();
            Keycosystem = new Keycosystem(new MonoGameObserver().UseKeyboard().UseMouse());
            Keyboard = new StandardKeyboard();
            Mouse = new StandardMouse();
            Director = new Director();
            DebugLogger = new DebugLogger { Hidden = true };
            Fps = new FpsCounter(120);
        }

        public static GameTimer Timer { get; private set; }

        public static Keycosystem Keycosystem { get; private set; }

        public static StandardKeyboard Keyboard { get; private set; }

        public static StandardMouse Mouse { get; private set; }

        public static Director Director { get; private set; }

        public static DebugLogger DebugLogger { get; private set; }

        public static FpsCounter Fps { get; private set; }
    }
}
