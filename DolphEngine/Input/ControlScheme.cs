using System;
using System.Collections.Generic;

namespace DolphEngine.Input
{
    public class ControlScheme
    {
        public IReadOnlyList<ControlReaction> Reactions => this._controls;
        private readonly List<ControlReaction> _controls = new List<ControlReaction>();

        public bool Enabled = true;

        public ControlScheme AddControl(Func<bool> condition, Action reaction)
        {
            this._controls.Add(new ControlReaction(condition, reaction));
            return this;
        }
    }
}
