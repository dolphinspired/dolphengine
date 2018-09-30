using System;
using System.Collections.Generic;
using System.Text;

namespace TacticsGame.Engine.Input.Conditions
{
    public static class While
    {
        #region Inputs

        public static KeyInputWhile Key(int key)
        {
            return new KeyInputWhile(key);
        }

        #endregion

        #region Conditions

        public static InputCondition IsHeld(KeyInputWhile input)
        {

        }

        public static InputCondition IsNotHeld(KeyInputWhile input)
        {

        }

        #endregion

        #region Nested classes

        public class KeyInputWhile
        {
            public KeyInputWhile(int key)
            {
                this.Key = key;
            }

            public int Key { get; }
        }

        #endregion
    }
}
