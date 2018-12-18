using DolphEngine.Input.Controls;
using DolphEngine.Input.State;
using System.Collections.Generic;

namespace DolphEngine.Input.Controllers
{
    public class StandardMouse : ControlBase
    {
        public bool LeftHanded;

        private Dictionary<string, SingleButtonControl> _buttonControls = new Dictionary<string, SingleButtonControl>();

        public StandardMouse()
        {
            this.SetKeys(
                InputKeys.MouseButton1,
                InputKeys.MouseButton2,
                InputKeys.MouseButton3,
                InputKeys.MouseButton4,
                InputKeys.MouseButton5,
                InputKeys.MouseCursorX,
                InputKeys.MouseCursorY,
                InputKeys.MouseScrollX,
                InputKeys.MouseScrollY
            );

            _buttonControls.Add(InputKeys.MouseButton1, new SingleButtonControl(InputKeys.MouseButton1));
            _buttonControls.Add(InputKeys.MouseButton2, new SingleButtonControl(InputKeys.MouseButton2));
            _buttonControls.Add(InputKeys.MouseButton3, new SingleButtonControl(InputKeys.MouseButton3));
            _buttonControls.Add(InputKeys.MouseButton4, new SingleButtonControl(InputKeys.MouseButton4));
            _buttonControls.Add(InputKeys.MouseButton5, new SingleButtonControl(InputKeys.MouseButton5));

            this.Cursor = new PositionalControl(InputKeys.MouseCursorX, InputKeys.MouseCursorY);
            this.Scroll = new PositionalControl(InputKeys.MouseScrollX, InputKeys.MouseScrollY);
        }

        public override void SetInputState(InputState inputState)
        {
            base.SetInputState(inputState);

            foreach (var control in this._buttonControls)
            {
                control.Value.SetInputState(inputState);
            }

            this.Cursor.SetInputState(inputState);
            this.Scroll.SetInputState(inputState);
        }

        public override void Update()
        {
            foreach (var control in this._buttonControls)
            {
                control.Value.Update();
            }

            this.Cursor.Update();
            this.Scroll.Update();
        }

        public SingleButtonControl PrimaryClick => LeftHanded ? _buttonControls[InputKeys.MouseButton2] : _buttonControls[InputKeys.MouseButton1];
        public SingleButtonControl SecondaryClick => LeftHanded ? _buttonControls[InputKeys.MouseButton1] : _buttonControls[InputKeys.MouseButton2];
        public SingleButtonControl MiddleClick => _buttonControls[InputKeys.MouseButton3];
        public SingleButtonControl SideButton1 => _buttonControls[InputKeys.MouseButton4];
        public SingleButtonControl SideButton2 => _buttonControls[InputKeys.MouseButton5];

        public readonly PositionalControl Cursor;
        public readonly PositionalControl Scroll;
    }
}
