using System;

namespace DolphEngine.Input.Controls
{
    public class OneAxisAnalogControl : ControlBase
    {
        private readonly string _key;

        private const float Idle = 0.00f;
        public const float DefaultSensitivity = 0.01f;
        public const float DefaultDeadzone = 0.25f;
        
        public OneAxisAnalogControl(string key, float sensitivity = DefaultSensitivity, float deadzone = DefaultDeadzone)
        {
            this._key = key;
            this.AddKey(key);

            this.Sensitivity = sensitivity;
            this.Deadzone = deadzone;
        }

        #region Event hooks

        public override void OnConnect()
        {
            this.LastTickMoved = this.Timer.Total.Ticks;
            this.LastTickPressed = this.Timer.Total.Ticks;
            this.LastTickReleased = this.Timer.Total.Ticks;
        }

        public override void OnUpdate()
        {
            if (!this.InputState.TryGetValue<float>(this._key, out var raw) || raw < this.Deadzone)
            {
                // Key is not registered or is sitting in the deadzone
                this.LastMagnitude = this.Magnitude;
                this.Magnitude = Idle;
                if (this.IsPressed)
                {
                    this.IsPressed = false;
                    this.LastTickReleased = this.Timer.Total.Ticks;
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
            this.LastTickMoved = this.Timer.Total.Ticks;

            if (!this.IsPressed)
            {
                this.IsPressed = true;
                this.LastTickPressed = this.Timer.Total.Ticks;
            }
        }

        #endregion

        public float Sensitivity;
        public float Deadzone;

        private float LastMagnitude;
        public float Magnitude { get; private set; }
        public long LastTickMoved { get; private set; }

        public float MagnitudeDelta => this.Magnitude - this.LastMagnitude;
        public float MagnitudeDeltaAbsolute => Math.Abs(this.MagnitudeDelta);
        public bool JustMoved => DurationHeld == 0;
        public long DurationHeld => !IsPressed ? -1 : this.Timer.Total.Ticks - LastTickMoved;

        public bool IsPressed { get; private set; }
        public long LastTickPressed { get; private set; }
        public long LastTickReleased { get; private set; }

        public bool JustPressed => DurationPressed == 0;
        public bool JustReleased => DurationReleased == 0;
        public long DurationPressed => !IsPressed ? -1 : this.Timer.Total.Ticks - LastTickPressed;
        public long DurationReleased => IsPressed ? -1 : this.Timer.Total.Ticks - LastTickReleased;
    }
}
