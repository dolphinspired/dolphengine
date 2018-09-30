using System;
using System.Collections.Generic;
using System.Text;

namespace TacticsGame.Engine.Input.Conditions
{
    public class InputCondition
    {
        public InputConditionType Type;

        public int Key;

        
    }

    public interface IKeyInputCondition
    {
        int Key { get; }

        int IsDown { get; }
    }

    public enum InputConditionType
    {
        Key = 1,
        Cursor = 2,
        Wheel = 3
    }
}
