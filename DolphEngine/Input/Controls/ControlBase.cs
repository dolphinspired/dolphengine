using DolphEngine.Input.State;
using System;
using System.Collections.Generic;

namespace DolphEngine.Input.Controls
{
    public abstract class ControlBase
    {
        protected InputState InputState;

        public virtual void SetInputState(InputState inputState)
        {
            if (this.InputState != null && inputState != null && this.InputState != inputState)
            {
                throw new InvalidOperationException($"This control is already tracked by another {nameof(InputState)}!");
            }

            this.InputState = inputState;
        }

        public IReadOnlyList<string> Keys { get; private set; }

        protected void SetKeys(params string[] keys)
        {
            this.Keys = new List<string>(keys);
        }

        public abstract void Update();
    }
}
