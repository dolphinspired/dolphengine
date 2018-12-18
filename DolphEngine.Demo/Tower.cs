using DolphEngine.Input;
using DolphEngine.MonoGame.Input;

namespace DolphEngine.Demo
{
    public static class Tower
    {
        public static void Initialize()
        {
            InitializeInput();
        }

        public static Keycosystem Keycosystem;

        private static void InitializeInput()
        {
            var observer = new MonoGameObserver().UseKeyboard();
            Keycosystem = new Keycosystem(observer);
        }
    }
}
