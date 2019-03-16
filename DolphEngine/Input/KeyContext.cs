using DolphEngine.Input.Controls;
using DolphEngine.Input.State;
using System;
using System.Collections.Generic;

namespace DolphEngine.Input
{
    public class KeyContext
    {
        private readonly List<ControlReaction> _controlReactions = new List<ControlReaction>();

        public KeyContext(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"A key context name is required!");
            }

            this.Name = name;
            this.Enabled = true;
        }

        public readonly string Name;
        public bool Enabled;

        public Keycosystem Keycosystem { get; internal set; }
        public IReadOnlyList<ControlReaction> ControlReactions => this._controlReactions;

        #region Public methods

        public KeyContext AddControl<T>(T control, Func<T, bool> condition, Action<T> reaction) where T : ControlBase
        {
            this.Keycosystem?.DeindexControls(this);
            this._controlReactions.Add(new ControlReaction<T>(control, condition, reaction));
            this.Keycosystem?.IndexControls(this);
            return this;
        }

        public KeyContext ClearControls()
        {
            this.Keycosystem?.DeindexControls(this);
            this._controlReactions.Clear();
            return this;
        }

        #endregion

        #region Non-public stuff

        internal KeyContext SetInputState(InputState state)
        {
            foreach (var cr in this._controlReactions)
            {
                cr.Control.SetInputState(state);
            }

            return this;
        }

        #endregion
    }
}
