using System;
using System.Collections.Generic;
using System.Text;

namespace TacticsGame.Engine.Input.Conditions
{
    public static class When
    {
        #region Inputs

        public static KeyInputWhen Key(int key)
        {
            return new KeyInputWhen(key);
        }

        public static CursorInputWhen Cursor()
        {
            return new CursorInputWhen();
        }

        public static WheelInputWhen ScrollWheel()
        {
            return new WheelInputWhen();
        }

        #endregion

        #region Conditions

        public static InputCondition IsPressed(this KeyInputWhen input)
        {

        }

        public static InputCondition IsReleased(this KeyInputWhen input)
        {

        }

        public static InputCondition Moves(this CursorInputWhen input)
        {

        }

        public static InputCondition Scrolls(this WheelInputWhen input)
        {

        }

        #endregion

        #region Nested classes

        public class KeyInputWhen
        {
            internal KeyInputWhen(int key)
            {
                this.Key = key;
            }

            public int Key { get; }
        }

        public class CursorInputWhen
        {
            internal CursorInputWhen() { }
        }

        public class WheelInputWhen
        {
            internal WheelInputWhen() { }
        }

        #endregion
    }
}
