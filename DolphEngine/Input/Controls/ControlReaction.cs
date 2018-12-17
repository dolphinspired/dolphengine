using System;

namespace DolphEngine.Input.Controls
{
    public abstract class ControlReaction
    {
        public abstract bool React(ControlBase control);
    }

    public class ControlReaction<T> : ControlReaction
        where T : ControlBase
    {
        public ControlReaction(Func<T, bool> condition, Action<T> reaction)
        {
            this.Condition = condition;
            this.Reaction = reaction;
        }

        public override bool React(ControlBase control)
        {
            var typed = control as T;

            if (this.Condition(typed))
            {
                this.Reaction(typed);
                return true;
            }

            return false;
        }

        public Func<T, bool> Condition
        {
            get => _condition;
            set => _condition = value ?? Disabled;
        }
        private Func<T, bool> _condition;
        private static bool Disabled(T control)
        {
            return false;
        }

        public Action<T> Reaction
        {
            get => _reaction;
            set => _reaction = value ?? DoNothing;
        }
        private Action<T> _reaction;
        private static void DoNothing(T control)
        {
        }
    }
}
