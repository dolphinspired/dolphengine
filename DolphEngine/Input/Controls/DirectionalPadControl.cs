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
        }

        public override void Update()
        {
            this.Up.Update();
            this.Right.Update();
            this.Down.Update();
            this.Left.Update();
        }

        public readonly SingleButtonControl Up;
        public readonly SingleButtonControl Right;
        public readonly SingleButtonControl Down;
        public readonly SingleButtonControl Left;
    }
}
