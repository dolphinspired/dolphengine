using DolphEngine.Input.State;

namespace DolphEngine.Input.Controls
{
    public class SingleButtonControl : ControlBase
    {
        public SingleButtonControl(string key)
        {
            this.SetKeys(key);
        }

        public override void Update()
        {
            var isPressed = InputState.GetValueOrDefault<bool>(this.Keys[0]);

            if (isPressed && !this.IsPressed)
            {
                this.IsPressed = true;
                this.LastTickPressed = this.InputState.CurrentTimestamp;
            }
            else if (!isPressed && this.IsPressed)
            {
                this.IsPressed = false;
                this.LastTickReleased = this.InputState.CurrentTimestamp;
            }
        }

        public bool IsPressed { get; private set; }
        public long LastTickPressed { get; private set; }
        public long LastTickReleased { get; private set; }

        public bool JustPressed => DurationPressed == 0;
        public bool JustReleased => DurationReleased == 0;
        public long DurationPressed => !IsPressed ? -1 : InputState.CurrentTimestamp - LastTickPressed;
        public long DurationReleased => IsPressed ? -1 : InputState.CurrentTimestamp - LastTickReleased;
    }
}
