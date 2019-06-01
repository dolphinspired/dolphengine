using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using Microsoft.Xna.Framework;

namespace DolphEngine.Demo
{
    public static class ControlSchemes
    {
        public static ControlScheme System(Game game, StandardKeyboard k)
        {
            return new ControlScheme()
                .AddControl(() => k.Escape.IsPressed, game.Exit);
        }

        public static ControlScheme DebugNavigation(DebugLogger debugLogger, StandardKeyboard k)
        {
            return new ControlScheme()
                .AddControl(() => k.OemTilde.JustPressed, () => debugLogger.Hidden = !debugLogger.Hidden)
                .AddControl(() => k.F1.JustPressed, () => debugLogger.PrevPage())
                .AddControl(() => k.F2.JustPressed, () => debugLogger.NextPage());
        }
    }
}
