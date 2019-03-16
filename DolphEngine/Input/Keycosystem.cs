using DolphEngine.Input.Controls;
using DolphEngine.Input.State;
using System;
using System.Collections.Generic;

namespace DolphEngine.Input
{
    public class Keycosystem
    {
        #region Private properties, indexes

        // An implementation of code that will update each key's state during the update loop
        private readonly IKeyStateObserver _observer;

        // Each key has exactly one state that gets updated one time per Update()
        //private readonly Dictionary<int, KeyState> _keyStatesByKey = new Dictionary<int, KeyState>();

        private readonly InputState _inputState = new InputState();
        private readonly Dictionary<string, InputKeyInfo> _inputKeys = new Dictionary<string, InputKeyInfo>();

        // Keeps track of the contexts in which each unique control is used
        // Only controls that are being used in a context will be updated
        private readonly Dictionary<ControlBase, HashSet<string>> _contextsByControl = new Dictionary<ControlBase, HashSet<string>>(ReferenceEqualityComparer<ControlBase>.Instance);
        private readonly Dictionary<string, HashSet<ControlBase>> _controlsByContext = new Dictionary<string, HashSet<ControlBase>>();

        private readonly Dictionary<string, KeyContext> _contextsByName = new Dictionary<string, KeyContext>();

        #endregion

        #region Constructors

        public Keycosystem(IKeyStateObserver observer)
        {
            this._observer = observer;
        }

        #endregion

        #region Update

        public void Update()
        {
            this._inputState.CurrentTimestamp = GameTimer.Global.Total.Ticks;
            
            // First, update the state of the observer (i.e. "initialize" it for this frame)
            this._observer.UpdateState();

            // Then, update all inputs that we're currently observing
            foreach (var inputKeyKvp in this._inputKeys)
            {
                var key = inputKeyKvp.Key;
                var parsed = inputKeyKvp.Value.Parsed;

                this._inputState.SetValue(key, this._observer.GetKeyValue(parsed));
            }

            // Update each unique control in the Keycosystem just once
            foreach (var control in this._contextsByControl.Keys)
            {
                control.Update();
            }

            // Then run all reactions for each enabled context in the order that they were added
            foreach (var contextName in this._controlsByContext.Keys)
            {
                var context = this._contextsByName[contextName];

                if (!context.Enabled)
                {
                    continue;
                }

                foreach (var reaction in context.ControlReactions)
                {
                    reaction.React();
                }
            }
        }

        #endregion

        #region Public methods - Context management (core)

        public Keycosystem AddContext<T>(T context) where T : KeyContext
        {
            if (this._contextsByName.ContainsKey(context.Name))
            {
                throw new ArgumentException($"A key context with name {context.Name} has already been added!");
            }

            context.SetInputState(this._inputState);
            this.IndexControls(context);
            
            this._contextsByName.Add(context.Name, context);
            context.Keycosystem = this;
            return this;
        }

        public bool TryGetContext(string contextName, out KeyContext context)
        {
            return this._contextsByName.TryGetValue(contextName, out context);
        }

        public KeyContext GetContext(string contextName)
        {
            return this._contextsByName[contextName];
        }

        public Keycosystem RemoveContext(string contextName)
        {
            if (!this._contextsByName.TryGetValue(contextName, out var context))
            {
                return this;
            }

            context.SetInputState(null);
            this.DeindexControls(context);

            this._contextsByName.Remove(contextName);
            context.Keycosystem = null;
            return this;
        }

        public Keycosystem ClearContexts()
        {
            foreach (var context in this._contextsByName.Values)
            {
                context.Keycosystem = null;
            }

            this._contextsByControl.Clear();
            this._controlsByContext.Clear();
            this._contextsByName.Clear();
            this._inputKeys.Clear();

            return this;
        }

        #endregion

        #region Public methods - Context management (derived)

        public Keycosystem RemoveContext<T>(T context) where T : KeyContext
        {
            return this.RemoveContext(context.Name);
        }

        public Keycosystem SwapContext<T>(T context) where T : KeyContext
        {
            if (this._contextsByName.TryGetValue(context.Name, out var oldContext))
            {
                this.RemoveContext(oldContext);
            }

            this.AddContext(context);
            return this;
        }

        #endregion

        #region Non-public stuff

        private class InputKeyInfo
        {
            public InputKeyInfo(InputKey parsed, int count)
            {
                this.Parsed = parsed;
                this.Count = count;
            }

            public InputKey Parsed;

            public int Count;
        }

        internal void IndexControls(KeyContext context)
        {
            if (!this._controlsByContext.TryGetValue(context.Name, out var controls))
            {
                controls = new HashSet<ControlBase>(ReferenceEqualityComparer<ControlBase>.Instance);
                this._controlsByContext.Add(context.Name, controls);
            }

            controls.Clear();
            foreach (var cr in context.ControlReactions)
            {
                controls.Add(cr.Control);
            }

            foreach (var cr in context.ControlReactions)
            {
                this.IndexByControl(context, cr.Control);

                foreach (var key in cr.Control.Keys)
                {
                    this.IndexByKey(context, key);
                }
            }
        }

        internal void DeindexControls(KeyContext context)
        {
            if (this._controlsByContext.ContainsKey(context.Name))
            {
                this._controlsByContext.Remove(context.Name);
            }

            foreach (var cr in context.ControlReactions)
            {
                this.DeindexByControl(context, cr.Control);

                foreach (var key in cr.Control.Keys)
                {
                    this.DeindexByKey(context, key);
                }
            }
        }

        private void IndexByKey(KeyContext context, string key)
        {
            if (this._inputKeys.ContainsKey(key))
            {
                // If it's already being tracked by another control or context, simply increment the counter
                this._inputKeys[key].Count++;
            }
            else
            {
                // Otherwise, add a new entry so it starts getting tracked
                this._inputKeys.Add(key, new InputKeyInfo(InputKey.Parse(key), 1));
            }
        }

        private void DeindexByKey(KeyContext context, string key)
        {
            if (this._inputKeys.TryGetValue(key, out var info))
            {
                // Decrement the number of controls tracking this key
                info.Count--;

                if (info.Count == 0)
                {
                    // If this was the last control to reference an input key, stop tracking that key's updates
                    this._inputKeys.Remove(key);
                }
            }
        }

        private void IndexByControl(KeyContext context, ControlBase control)
        {
            if (this._contextsByControl.TryGetValue(control, out var contexts))
            {
                contexts.Add(context.Name);
            }
            else
            {
                this._contextsByControl.Add(control, new HashSet<string> { context.Name });
            }
        }

        private void DeindexByControl(KeyContext context, ControlBase control)
        {
            if (this._contextsByControl.TryGetValue(control, out var contextNames))
            {
                contextNames.Remove(context.Name);

                if (contextNames.Count == 0)
                {
                    this._contextsByControl.Remove(control);
                }
            }
        }

        #endregion
    }
}
