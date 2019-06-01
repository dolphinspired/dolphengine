using DolphEngine.Input.Controls;

namespace DolphEngine.Input.Controllers
{
    public class StandardMouse : ControlBase
    {
        // Set to 'true' to swap Primary/Secondary click
        public bool LeftHanded;

        public StandardMouse()
        {
            this._button1 = this.AddControl(new SingleButtonControl(InputKeys.MouseButton1));
            this._button2 = this.AddControl(new SingleButtonControl(InputKeys.MouseButton2));
            this.MiddleClick = this.AddControl(new SingleButtonControl(InputKeys.MouseButton3));
            this.SideButton1 = this.AddControl(new SingleButtonControl(InputKeys.MouseButton4));
            this.SideButton2 = this.AddControl(new SingleButtonControl(InputKeys.MouseButton5));

            this.Cursor = this.AddControl(new TwoAxisPositionalControl(InputKeys.MouseCursorX, InputKeys.MouseCursorY));
            this.Scroll = this.AddControl(new TwoAxisPositionalControl(InputKeys.MouseScrollX, InputKeys.MouseScrollY));
        }

        private readonly SingleButtonControl _button1;
        private readonly SingleButtonControl _button2;

        public SingleButtonControl PrimaryClick => LeftHanded ? _button2 : _button1;
        public SingleButtonControl SecondaryClick => LeftHanded ? _button1 : _button2;
        public SingleButtonControl MiddleClick { get; private set; }
        public SingleButtonControl SideButton1 { get; private set; }
        public SingleButtonControl SideButton2 { get; private set; }

        public readonly TwoAxisPositionalControl Cursor;
        public readonly TwoAxisPositionalControl Scroll;
    }
}
