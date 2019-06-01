using DolphEngine.Input;
using DolphEngine.Input.Controllers;

namespace DolphEngine.Demo
{
    public static class DebugPages
    {
        public static DebugLogger AddControlInfo(this DebugLogger debug, Keycosystem keycosystem)
        {
            var keyboard = keycosystem.GetController<StandardKeyboard>(1);
            var mouse = keycosystem.GetController<StandardMouse>(1);

            debug.AddPage(
                () => "Control A:",
                () => $"IsPressed: {keyboard.A.IsPressed}, LastTickPressed: {keyboard.A.LastTickPressed}, LastTickReleased: {keyboard.A.LastTickReleased}",
                DebugLogger.EmptyLine,
                () => "Control Z:",
                () => $"IsPressed: {keyboard.Z.IsPressed}, LastTickPressed: {keyboard.Z.LastTickPressed}, LastTickReleased: {keyboard.Z.LastTickReleased}",
                DebugLogger.EmptyLine,
                () => "Arrow keys:",
                () => $"IsPressed: {keyboard.ArrowKeys.IsPressed}, LastTickPressed: {keyboard.ArrowKeys.LastTickPressed}, LastTickReleased: {keyboard.ArrowKeys.LastTickReleased}",
                () => $"Direction: {keyboard.ArrowKeys.Direction}, LastTickDirectionChanged: {keyboard.ArrowKeys.LastTickDirectionChanged}, DirectionHeld: {keyboard.ArrowKeys.DurationDirectionHeld}",
                DebugLogger.EmptyLine,
                () => "Mouse:",
                () => $"PrimaryClick: {mouse.PrimaryClick.IsPressed}, SecondaryClick: {mouse.SecondaryClick.IsPressed}, LeftHanded: {mouse.LeftHanded}",
                () => $"X: {mouse.Cursor.X}, Y: {mouse.Cursor.Y}",
                () => $"Scroll X: {mouse.Scroll.X}, Scroll Y: {mouse.Scroll.Y}, Scroll Click: {mouse.MiddleClick.IsPressed}"
            );

            return debug;
        }
    }
}
