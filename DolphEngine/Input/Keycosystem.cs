using DolphEngine.Input.Controls;
using DolphEngine.Input.State;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly Dictionary<ControlBase, ControlReaction> _controlReactions = new Dictionary<ControlBase, ControlReaction>();

        // Each key is indexed by 1-or-more handlers that are triggered by it
        // Each handler is indexed by 0-or-more keys that trigger them
        //private readonly Dictionary<int, HashSet<KeyHandler>> _handlersByKey = new Dictionary<int, HashSet<KeyHandler>>();
        //private readonly Dictionary<KeyHandler, HashSet<int>> _keysByHandler = new Dictionary<KeyHandler, HashSet<int>>(ReferenceEqualityComparer<KeyHandler>.Instance);

        //private readonly List<KeyHandler> _handlersByExecutionOrder = new List<KeyHandler>();

        #endregion

        #region Constructors

        public Keycosystem(IKeyStateObserver observer)
        {
            this._observer = observer;
        }

        #endregion

        #region Update

        public void Update(long gameTick)
        {
            this._inputState.CurrentTimestamp = gameTick;
            
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
            foreach (var reactionKvp in this._controlReactions)
            {
                var control = reactionKvp.Key;
                var reaction = reactionKvp.Value;

                control.Update();
                reaction.React(control);
            }
        }

        #endregion

        #region Add/Remove Reactions

        public Keycosystem AddReaction<T>(T control, Func<T, bool> condition, Action<T> reaction) where T : ControlBase
        {
            control.SetInputState(this._inputState);
            var cr = new ControlReaction<T>(condition, reaction);
            this._controlReactions.Add(control, cr);

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
