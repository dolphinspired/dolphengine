using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Input
{
    public class KeyHandler
    {
        #region Properties and data structures

        public Keycosystem Keycosystem { get; internal set; }
        public bool Enabled = true;

        private readonly Dictionary<int, uint> _reactionIdsByKey = new Dictionary<int, uint>();
        private readonly Dictionary<uint, InputReaction> _reactionsById = new Dictionary<uint, InputReaction>();
        private ushort _nextReaction;

        private class InputReaction
        {
            public KeyCondition Condition;

            public Func<KeyState, bool> CustomCondition;

            public Action<KeyState> Action;
        }

        #endregion

        #region Update

        public void Update(long gameTick, Dictionary<int, KeyState> keyStatesByKey)
        {
            if (!this.Enabled)
            {
                return;
            }

            foreach (var reactionIdByKey in this._reactionIdsByKey)
            {
                var keyState = keyStatesByKey[reactionIdByKey.Key];
                var reaction = this._reactionsById[reactionIdByKey.Value];

                bool shouldInvoke;

                if (reaction.Condition == KeyCondition.Custom)
                {
                    // Evaluate custom condition
                    shouldInvoke = reaction.CustomCondition.Invoke(keyState);
                }
                else
                {
                    // Evaluate preset condition
                    switch (reaction.Condition)
                    {
                        case KeyCondition.Always:
                            shouldInvoke = true;
                            break;
                        case KeyCondition.WhenPressed:
                            shouldInvoke = keyState.IsPressed && keyState.IsPressedLastChange == gameTick;
                            break;
                        case KeyCondition.WhilePressed:
                            shouldInvoke = keyState.IsPressed;
                            break;
                        case KeyCondition.WhenReleased:
                            shouldInvoke = !keyState.IsPressed && keyState.IsPressedLastChange == gameTick;
                            break;
                        case KeyCondition.WhileReleased:
                            shouldInvoke = !keyState.IsPressed;
                            break;
                        case KeyCondition.WhenDigitalChanged:
                            shouldInvoke = keyState.DigitalLastChange == gameTick;
                            break;
                        case KeyCondition.WhenAnalogChanged:
                            shouldInvoke = keyState.AnalogLastChange == gameTick;
                            break;
                        default:
                            throw new InvalidOperationException($"Unhandled input condition: {reaction.Condition} ({(int)reaction.Condition})");
                    }
                }

                if (shouldInvoke)
                {
                    reaction.Action.Invoke(keyState);
                }
            }
        }

        #endregion

        #region Binding

        /// <summary>
        /// Tells this <see cref="KeyHandler"/> to execute the specified action when a condition is met for this key.
        /// </summary>
        /// <param name="key">The key to bind to this action</param>
        /// <param name="condition">The preset condition of this key under which this action should be executed</param>
        /// <param name="action">The action to execute when this key condition is met</param>
        public KeyHandler BindKey(int key, KeyCondition condition, Action<KeyState> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (condition == KeyCondition.Custom)
            {
                throw new ArgumentException($"Cannot set condition '{KeyCondition.Custom}'. A custom condition must be provided!");
            }

            var reactionId = ++this._nextReaction;
            var reaction = new InputReaction
            {
                Condition = condition,
                Action = action
            };
            this._reactionsById.Add(reactionId, reaction);

            this.BindKeyToReaction(key, reactionId);
            return this;
        }

        /// <summary>
        /// Tells this <see cref="KeyHandler"/> to execute the specified action when a condition is met for this key.
        /// </summary>
        /// <param name="key">The key to bind to this action</param>
        /// <param name="condition">The custom condition of this key under which this action should be executed</param>
        /// <param name="action">The action to execute when this key condition is met</param>
        public KeyHandler BindKey(int key, Func<KeyState, bool> condition, Action<KeyState> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var reactionId = ++this._nextReaction;
            var reaction = new InputReaction
            {
                Condition = KeyCondition.Custom,
                CustomCondition = condition,
                Action = action
            };
            this._reactionsById.Add(reactionId, reaction);

            this.BindKeyToReaction(key, reactionId);
            return this;
        }

        /// <summary>
        /// Binds the reaction from one input to one or more other inputs.
        /// </summary>
        /// <param name="fromKey">The key currently bound to the action to copy</param>
        /// <param name="toKeys">The additional keys to bind to that action</param>
        public KeyHandler CopyKey(int fromKey, params int[] toKeys)
        {
            if (!this._reactionIdsByKey.TryGetValue(fromKey, out var reactionId))
            {
                throw new ArgumentException($"There is no reaction to copy from key {fromKey}!");
            }

            if (toKeys == null || !toKeys.Any())
            {
                throw new ArgumentException($"No keys were specified to copy to!");
            }

            foreach (var key in toKeys)
            {
                this.BindKeyToReaction(key, reactionId);
            }

            return this;
        }

        /// <summary>
        /// Binds the reaction from one input to another input, and unbinds it from the original input.
        /// </summary>
        /// <param name="fromKey">The key currently bound to the action</param>
        /// <param name="toKey">The new key to bind to that action</param>
        public KeyHandler RebindKey(int fromKey, int toKey)
        {
            this.CopyKey(fromKey, toKey);
            return this.UnbindKey(fromKey);
        }

        /// <summary>
        /// Unbinds a key so that it no longer triggers a reaction.
        /// </summary>
        /// <param name="key">The key that should no longer trigger a reaction</param>
        public KeyHandler UnbindKey(int key)
        {
            if (this._reactionIdsByKey.ContainsKey(key))
            {
                this._reactionIdsByKey.Remove(key);
            }

            this.Keycosystem?.NotifyKeyUnbound(this, key);
            return this;
        }

        #endregion

        #region Non-public methods

        /// <summary>
        /// Binds an action that has already been bound to a key to the specified key as well.
        /// </summary>
        /// <param name="key">The key that will trigger the action</param>
        /// <param name="reactionId">The id of the action to run when the key is triggered</param>
        private KeyHandler BindKeyToReaction(int key, uint reactionId)
        {
            if (this._reactionIdsByKey.ContainsKey(key))
            {
                this._reactionIdsByKey[key] = reactionId;
            }
            else
            {
                this._reactionIdsByKey.Add(key, reactionId);
            }

            this.Keycosystem?.NotifyKeyBound(this, key);
            return this;
        }

        /// <summary>
        /// Only for initial registration of a handler. Game code should have no reason to check this.
        /// </summary>
        internal IEnumerable<int> GetAllHandledKeys()
        {
            return this._reactionIdsByKey.Select(x => x.Key);
        }

        #endregion
    }

    
}
