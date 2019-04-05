using DolphEngine.Input.State;

namespace DolphEngine.Input.Controls
{
    public class DirectionalPadControl : ControlBase
    {
        public DirectionalPadControl(string upKey, string rightKey, string downKey, string leftKey)
        {
            this.Up = new SingleButtonControl(upKey);
            this.Right = new SingleButtonControl(rightKey);
            this.Down = new SingleButtonControl(downKey);
            this.Left = new SingleButtonControl(leftKey);
            this.SetKeys(upKey, rightKey, downKey, leftKey);
        }

        public override void SetInputState(InputState inputState)
        {
            base.SetInputState(inputState);
            this.Up.SetInputState(inputState);
            this.Right.SetInputState(inputState);
            this.Down.SetInputState(inputState);
            this.Left.SetInputState(inputState);

            this.LastTickDirectionChanged = inputState?.CurrentTimestamp ?? 0;
        }

        public override void Update()
        {
            this.Up.Update();
            this.Right.Update();
            this.Down.Update();
            this.Left.Update();

            // Are any of the arrow keys pressed?
            var isPressed = this.Up.IsPressed || this.Right.IsPressed || this.Down.IsPressed || this.Left.IsPressed;

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
                this.LastTickDirectionChanged = InputState.CurrentTimestamp;
            }
        }

        public readonly SingleButtonControl Up;
        public readonly SingleButtonControl Right;
        public readonly SingleButtonControl Down;
        public readonly SingleButtonControl Left;

        public Direction2d Direction { get; private set; }
        public long LastTickDirectionChanged { get; private set; }

        public bool DirectionJustChanged => LastTickDirectionChanged == 0;
        public long DurationDirectionHeld => InputState.CurrentTimestamp - LastTickDirectionChanged;

        public bool IsPressed { get; private set; }
        public long LastTickPressed { get; private set; }
        public long LastTickReleased { get; private set; }

        public bool JustPressed => DurationPressed == 0;
        public bool JustReleased => DurationReleased == 0;
        public long DurationPressed => !IsPressed ? -1 : InputState.CurrentTimestamp - LastTickPressed;
        public long DurationReleased => IsPressed ? -1 : InputState.CurrentTimestamp - LastTickReleased;
    }
}
