using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using Microsoft.Xna.Framework;

namespace DolphEngine.Demo
{
    public static class ControlContexts
    {
        public static readonly StandardKeyboard Keyboard = new StandardKeyboard();
        public static readonly StandardMouse Mouse = new StandardMouse();
        
        public static KeyContext System(Game game)
        {
            return new KeyContext("System")
                .AddControl(Keyboard, k => k.Escape.IsPressed, k => game.Exit());
        }

        public static KeyContext DebugNavigation()
        {
            return new KeyContext("DebugLogger")
                .AddControl(Keyboard, k => k.OemTilde.JustPressed, k => Tower.DebugLogger.Hidden = !Tower.DebugLogger.Hidden)
                .AddControl(Keyboard, k => k.F1.JustPressed, k => Tower.DebugLogger.PrevPage())
                .AddControl(Keyboard, k => k.F2.JustPressed, k => Tower.DebugLogger.NextPage());
        }
    }
}
