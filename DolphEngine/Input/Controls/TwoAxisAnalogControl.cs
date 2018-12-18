using DolphEngine.Input.State;
using System;

namespace DolphEngine.Input.Controls
{
    public class TwoAxisAnalogControl : ControlBase
    {
        private const float Idle = 0.00f;

        private const float DiagonalBuffer = 11.25f;
        private const float UpRight = 45.00f;
        private const float DownRight = 135.00f;
        private const float DownLeft = 225.00f;
        private const float UpLeft = 315.00f;

        public TwoAxisAnalogControl(string xKey, string yKey)
        {
            this.X = new OneAxisAnalogControl(xKey);
            this.Y = new OneAxisAnalogControl(yKey);
            this.SetKeys(xKey, yKey);
        }

        public TwoAxisAnalogControl(string xKey, string yKey, float sensitivity, float deadzone)
        {
            this.X = new OneAxisAnalogControl(xKey, sensitivity, deadzone);
            this.Y = new OneAxisAnalogControl(yKey, sensitivity, deadzone);
        }

        public override void SetInputState(InputState inputState)
        {
            base.SetInputState(inputState);
            this.X.SetInputState(inputState);
            this.Y.SetInputState(inputState);
        }

        public override void Update()
        {
            this.X.Update();
            this.Y.Update();

            var isPressed = this.X.IsPressed || this.Y.IsPressed;

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

            this.LastAngle = this.Angle;
            this.LastMagnitude = this.Magnitude;

            if (isPressed)
            {
                var angle = (float)Math.Atan2(Y.Magnitude, X.Magnitude);
                this.Angle = angle;
                this.Magnitude = (float)Math.Sqrt(Math.Pow(X.Magnitude, 2) + Math.Pow(Y.Magnitude, 2));
                this.Direction = Direction.None; // todo: calculate direction
                // todo: track changes (last tick moved)
            }
            else
            {
                this.Angle = Idle;
                this.Magnitude = Idle;
                this.Direction = Direction.None;
            }
        }
        
        public readonly OneAxisAnalogControl X;
        public readonly OneAxisAnalogControl Y;

        private float LastAngle;
        public float Angle { get; private set; }
        public Direction Direction { get; private set; }

        public float AngleDelta => this.Angle - this.LastAngle;
        public float AngleDeltaAbsolute => Math.Abs(this.AngleDelta);

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
