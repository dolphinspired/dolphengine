using System;
using System.Collections.Generic;
using System.Linq;

namespace TacticsGame.Engine.Input
{
    public class KeyHandler
    {
        public Keycosystem Keycosystem { get; internal set; }
        public bool Enabled = true;

        private readonly Dictionary<int, uint> _actionIdsByKey = new Dictionary<int, uint>();
        private readonly Dictionary<uint, Action> _actionsById = new Dictionary<uint, Action>();
        private ushort _nextAction;

        public void Update(Dictionary<int, KeyState> keyStatesByKey)
        {
            if (!this.Enabled)
            {
                return;
            }

            foreach (var actionIdByKey in this._actionIdsByKey)
            {
                var key = actionIdByKey.Key;
                var actionId = actionIdByKey.Value;

                var keyState = keyStatesByKey[key];

            }
        }

        #region Lookup

        // Only for initial registration of a handler. Game code should have no reason to check this.
        internal IEnumerable<int> GetAllHandledKeys()
        {
            return this._actionIdsByKey.Select(x => x.Key);
        }

        public uint GetActionIdForKey(int key)
        {
            return this._actionIdsByKey[key];
        }

        public bool TryGetActionIdForKey(int key, out uint actionId)
        {
            if (!this._actionIdsByKey.TryGetValue(key, out var indexedActionId))
            {
                actionId = 0;
                return false;
            }

            actionId = indexedActionId;
            return true;
        }

        public Action GetActionById(uint actionId)
        {
            return this._actionsById[actionId];
        }

        public bool TryGetActionById(uint actionId, out Action action)
        {
            if (!this._actionsById.TryGetValue(actionId, out var indexedAction))
            {
                action = null;
                return false;
            }

            action = indexedAction;
            return true;
        }

        public Action GetActionForKey(int key)
        {
            return this._actionsById[this._actionIdsByKey[key]];
        }

        public bool TryGetActionForKey(int key, out Action action)
        {
            if (!this._actionIdsByKey.TryGetValue(key, out var actionId))
            {
                action = null;
                return false;
            }

            if (!this._actionsById.TryGetValue(actionId, out var indexedAction))
            {
                action = null;
                return false;
            }

            action = indexedAction;
            return true;
        }

        #endregion

        #region Binding

        /// <summary>
        /// Binds a key to the specified action.
        /// </summary>
        /// <param name="key">The key that will trigger the action</param>
        /// <param name="action">The action to run when the key is triggered</param>
        /// <param name="allowRebind">Should this action overwrite a key's current bound action, if it has one? Default: true</param>
        public KeyHandler BindKey(int key, Action action, bool allowRebind = true)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var actionId = ++this._nextAction;
            this._actionsById.Add(actionId, action);

            this.BindKey(key, actionId, allowRebind);
            return this;
        }

        /// <summary>
        /// Binds an action that has already been bound to a key to the specified key as well.
        /// </summary>
        /// <param name="key">The key that will trigger the action</param>
        /// <param name="actionId">The id of the action to run when the key is triggered</param>
        /// <param name="allowRebind">Should this action overwrite a key's current bound action, if it has one? Default: true</param>
        public KeyHandler BindKey(int key, uint actionId, bool allowRebind = true)
        {
            if (!this._actionsById.ContainsKey(actionId))
            {
                throw new ArgumentException(nameof(actionId), $"No action has been registered to id {actionId}!");
            }

            if (this._actionIdsByKey.ContainsKey(key))
            {
                if (!allowRebind)
                {
                    throw new ArgumentException(nameof(key), $"Key {key} is already bound to an action!");
                }

                this._actionIdsByKey[key] = actionId;
            }
            else
            {
                this._actionIdsByKey.Add(key, actionId);
            }

            this.Keycosystem?.NotifyKeyBound(this, key);
            return this;
        }

        /// <summary>
        /// Unbinds a key so that it no longer triggers an action.
        /// </summary>
        /// <param name="key">The key that should no longer trigger an action</param>
        public KeyHandler UnbindKey(int key)
        {
            if (this._actionIdsByKey.ContainsKey(key))
            {
                this._actionIdsByKey.Remove(key);
            }

            this.Keycosystem?.NotifyKeyUnbound(this, key);
            return this;
        }

        /// <summary>
        /// Unbinds an action so that all keys bound to it no longer trigger an action, and the action
        /// can no longer be assigned to any keys.
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public KeyHandler UnbindAction(uint actionId)
        {
            if (!this._actionsById.ContainsKey(actionId))
            {
                return this;
            }

            this._actionsById.Remove(actionId);

            // I'm not sure if I need ToList() here, but I'm afraid of removing from an enumerable while I'm iterating over it
            var keysToUnbind = this._actionIdsByKey.Where(x => x.Value == actionId).Select(x => x.Key).ToList();
            foreach (var key in keysToUnbind)
            {
                this._actionIdsByKey.Remove(key);
                this.Keycosystem?.NotifyKeyUnbound(this, key);
            }

            return this;
        }

        #endregion
    }
}
