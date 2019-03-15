using DolphEngine.Input.Controls;
using DolphEngine.Input.State;
using System;
using System.Collections.Generic;

namespace DolphEngine.Input
{
    public class Keycosystem
    {
        #region Private properties, indexes

        private readonly GameTimer _timer;

        // An implementation of code that will update each key's state during the update loop
        private readonly IKeyStateObserver _observer;

        // Each key has exactly one state that gets updated one time per Update()
        //private readonly Dictionary<int, KeyState> _keyStatesByKey = new Dictionary<int, KeyState>();

        private readonly InputState _inputState = new InputState();
        private readonly Dictionary<string, InputKeyInfo> _inputKeys = new Dictionary<string, InputKeyInfo>();

        private readonly Dictionary<ControlBase, List<ControlReaction>> _controlReactionsByControl =
            new Dictionary<ControlBase, List<ControlReaction>>(ReferenceEqualityComparer<ControlBase>.Instance);

        // Each key is indexed by 1-or-more handlers that are triggered by it
        // Each handler is indexed by 0-or-more keys that trigger them
        //private readonly Dictionary<int, HashSet<KeyHandler>> _handlersByKey = new Dictionary<int, HashSet<KeyHandler>>();
        //private readonly Dictionary<KeyHandler, HashSet<int>> _keysByHandler = new Dictionary<KeyHandler, HashSet<int>>(ReferenceEqualityComparer<KeyHandler>.Instance);

        //private readonly List<KeyHandler> _handlersByExecutionOrder = new List<KeyHandler>();

        #endregion

        #region Constructors

        public Keycosystem(GameTimer timer, IKeyStateObserver observer)
        {
            this._timer = timer;
            this._observer = observer;
        }

        #endregion

        #region Update

        public void Update()
        {
            this._inputState.CurrentTimestamp = this._timer.Total.Ticks;
            
            // First, update the state of the observer (i.e. "initialize" it for this frame)
            this._observer.UpdateState();

            // Then, update all inputs that we're currently observing
            foreach (var inputKeyKvp in this._inputKeys)
            {
                var key = inputKeyKvp.Key;
                var parsed = inputKeyKvp.Value.Parsed;

                this._inputState.SetValue(key, this._observer.GetKeyValue(parsed));
            }

            // Finally, execute the on-update reactions for each control
            foreach (var kvp in this._controlReactionsByControl)
            {
                var control = kvp.Key;
                control.Update();

                foreach (var reaction in kvp.Value)
                {
                    reaction.React(control);
                }
            }
        }

        #endregion

        #region Add/Remove Reactions

        public Keycosystem AddControlReaction<T>(T control, Func<T, bool> condition, Action<T> reaction) where T : ControlBase
        {
            control.SetInputState(this._inputState);
            var cr = new ControlReaction<T>(condition, reaction);
            
            // Index the reaction by which control it's bound to
            if (this._controlReactionsByControl.ContainsKey(control))
            {
                // If another reaction has already been added to this control, add this reaction to the list
                this._controlReactionsByControl[control].Add(cr);
            }
            else
            {
                // Otherwise, add a new index entry for this control, and create a list with this reaction in it
                this._controlReactionsByControl.Add(control, new List<ControlReaction> { cr });
            }

            // Add this control's keys to the inputs that we track on each update
            foreach (var key in control.Keys)
            {
                if (this._inputKeys.ContainsKey(key))
                {
                    // If it's already being tracked by another control, simply increment the counter
                    this._inputKeys[key].Count++;
                }
                else
                {
                    // Otherwise, add a new entry so it starts getting tracked
                    this._inputKeys.Add(key, new InputKeyInfo(InputKey.Parse(key), 1));
                }
            }

            return this;
        }

        public Keycosystem RemoveControl(ControlBase control)
        {
            foreach (var key in control.Keys)
            {
                // Decrement the number of controls tracking this key
                var info = this._inputKeys[key];
                info.Count--;

                if (info.Count == 0)
                {
                    // If this was the last control to reference an input key, stop tracking that key's updates
                    this._inputKeys.Remove(key);
                }
            }

            this._controlReactionsByControl.Remove(control);
            return this;
        }

        public Keycosystem RebindControl<T>(T from, T to) where T : ControlBase
        {
            throw new NotImplementedException();

            return this;
        }

        public Keycosystem ClearControls()
        {
            this._controlReactionsByControl.Clear();
            this._inputKeys.Clear();

            return this;
        }

        #endregion

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
    }
}
