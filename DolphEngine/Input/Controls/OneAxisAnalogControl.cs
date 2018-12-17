using DolphEngine.Input.State;
using System;

namespace DolphEngine.Input.Controls
{
    public class OneAxisAnalogControl : ControlBase
    {
        private const float Idle = 0.00f;
        private const float DefaultSensitivity = 0.01f;
        private const float DefaultDeadzone = 0.25f;

        public OneAxisAnalogControl(string key, float sensitivity = DefaultSensitivity, float deadzone = DefaultDeadzone)
        {
            this.SetKeys(key);
            this.Sensitivity = sensitivity;
            this.Deadzone = deadzone;
        }

        public override void Update()
        {
            if (!this.InputState.TryGetValue<float>(this.Keys[0], out var raw) || raw < this.Deadzone)
            {
                // Key is not registered or is sitting in the deadzone
                this.LastMagnitude = this.Magnitude;
                this.Magnitude = Idle;
                if (this.IsPressed)
                {
                    this.IsPressed = false;
                    this.LastTickReleased = this.InputState.CurrentTimestamp;
                }
                return;
            }
            
            if (Math.Abs(raw - this.Magnitude) < this.Sensitivity)
            {
                // Key is pressed (outside of deadzone), but did not move significantly
                return;
            }

            // Key is pressed (outside of deadzone) and moved significantly
            this.LastMagnitude = this.Magnitude;
            this.Magnitude = raw;
            this.LastTickMoved = this.InputState.CurrentTimestamp;

            if (!this.IsPressed)
            {
                this.IsPressed = true;
                this.LastTickPressed = this.InputState.CurrentTimestamp;
            }
        }
        
        public float Sensitivity;
        public float Deadzone;

        private float LastMagnitude;
        public float Magnitude { get; private set; }
        public long LastTickMoved { get; private set; }

        public float MagnitudeDelta => this.Magnitude - this.LastMagnitude;
        public float MagnitudeDeltaAbsolute => Math.Abs(this.MagnitudeDelta);
        public bool JustMoved => DurationHeld == 0;
        public long DurationHeld => !IsPressed ? -1 : InputState.CurrentTimestamp - LastTickMoved;

        public bool IsPressed { get; private set; }
        public long LastTickPressed { get; private set; }
        public long LastTickReleased { get; private set; }

        public bool JustPressed => DurationPressed == 0;
        public bool JustReleased => DurationReleased == 0;
        public long DurationPressed => !IsPressed ? -1 : InputState.CurrentTimestamp - LastTickPressed;
        public long DurationReleased => IsPressed ? -1 : InputState.CurrentTimestamp - LastTickReleased;
    }
}
