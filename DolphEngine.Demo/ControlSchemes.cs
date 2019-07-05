using DolphEngine.Graphics;
using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.Input.Controls;

namespace DolphEngine.Demo
{
    public static class ControlSchemes
    {
        public static ControlScheme DebugNavigation(DebugLogger debugLogger, StandardKeyboard k)
        {
            return new ControlScheme()
                .AddControl(() => k.OemTilde.JustPressed, () => debugLogger.Hidden = !debugLogger.Hidden)
                .AddControl(() => k.F1.JustPressed, () => debugLogger.PrevPage())
                .AddControl(() => k.F2.JustPressed, () => debugLogger.NextPage());
        }

        public static void PanCamera(Viewport2d camera, DirectionalPadControl dpad)
        {
            if ((dpad.Direction & Direction2d.Up) > 0)
            {
                camera.Space.Y -= 8;
            }
            if ((dpad.Direction & Direction2d.Right) > 0)
            {
                camera.Space.X += 8;
            }
            if ((dpad.Direction & Direction2d.Down) > 0)
            {
                camera.Space.Y += 8;
            }
            if ((dpad.Direction & Direction2d.Left) > 0)
            {
                camera.Space.X -= 8;
            }
        }
    }
}
