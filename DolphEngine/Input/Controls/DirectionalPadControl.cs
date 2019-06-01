namespace DolphEngine.Input.Controls
{
    public class DirectionalPadControl : ControlBase
    {
        public DirectionalPadControl(string upKey, string rightKey, string downKey, string leftKey)
        {
            this.Up = this.AddControl(new SingleButtonControl(upKey));
            this.Right = this.AddControl(new SingleButtonControl(rightKey));
            this.Down = this.AddControl(new SingleButtonControl(downKey));
            this.Left = this.AddControl(new SingleButtonControl(leftKey));
        }

        #region Event hooks

        public override void OnConnect()
        {
            this.LastTickDirectionChanged = this.Timer.Total.Ticks;
            this.LastTickPressed = this.Timer.Total.Ticks;
            this.LastTickReleased = this.Timer.Total.Ticks;
        }

        public override void OnUpdate()
        {
            // Are any of the arrow keys pressed?
            var isPressed = this.Up.IsPressed || this.Right.IsPressed || this.Down.IsPressed || this.Left.IsPressed;

            if (isPressed && !this.IsPressed)
            {
                this.IsPressed = true;
                this.LastTickPressed = this.Timer.Total.Ticks;
            }
            else if (!isPressed && this.IsPressed)
            {
                this.IsPressed = false;
                this.LastTickReleased = this.Timer.Total.Ticks;
            }

            // What direction do the pressed arrow keys form?
            var direction = Direction2d.None;

            if (isPressed)
            {
                if (this.Up.IsPressed)
                {
                    direction |= Direction2d.Up;
                }
                if (this.Right.IsPressed)
                {
                    direction |= Direction2d.Right;
                }
                if (this.Down.IsPressed)
                {
                    direction |= Direction2d.Down;
                }
                if (this.Left.IsPressed)
                {
                    direction |= Direction2d.Left;
                }
            }

            if (direction != this.Direction)
            {
                this.Direction = direction;
                this.LastTickDirectionChanged = this.Timer.Total.Ticks;
            }
        }

        #endregion

        public readonly SingleButtonControl Up;
        public readonly SingleButtonControl Right;
        public readonly SingleButtonControl Down;
        public readonly SingleButtonControl Left;

        public Direction2d Direction { get; private set; }
        public long LastTickDirectionChanged { get; private set; }

        public bool DirectionJustChanged => LastTickDirectionChanged == 0;
        public long DurationDirectionHeld => this.Timer.Total.Ticks - LastTickDirectionChanged;

        public bool IsPressed { get; private set; }
        public long LastTickPressed { get; private set; }
        public long LastTickReleased { get; private set; }

        public bool JustPressed => DurationPressed == 0;
        public bool JustReleased => DurationReleased == 0;
        public long DurationPressed => !IsPressed ? -1 : this.Timer.Total.Ticks - LastTickPressed;
        public long DurationReleased => IsPressed ? -1 : this.Timer.Total.Ticks - LastTickReleased;
    }
}
