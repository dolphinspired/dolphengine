using System;

namespace DolphEngine.Input
{
    public class ControlReaction
    {
        public ControlReaction(Func<bool> condition, Action reaction)
        {
            this.Condition = condition;
            this.Reaction = reaction;
        }

        public readonly Func<bool> Condition;

        public readonly Action Reaction;
    }
}
