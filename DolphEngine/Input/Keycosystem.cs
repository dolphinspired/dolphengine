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
        private readonly Dictionary<int, KeyState> _keyStatesByKey = new Dictionary<int, KeyState>();

        // Each key is indexed by 1-or-more handlers that are triggered by it
        // Each handler is indexed by 0-or-more keys that trigger them
        private readonly Dictionary<int, HashSet<KeyHandler>> _handlersByKey = new Dictionary<int, HashSet<KeyHandler>>();
        private readonly Dictionary<KeyHandler, HashSet<int>> _keysByHandler = new Dictionary<KeyHandler, HashSet<int>>(ReferenceEqualityComparer<KeyHandler>.Instance);

        private readonly List<KeyHandler> _handlersByExecutionOrder = new List<KeyHandler>();

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
            // First, update the state of the observer (i.e. "initialize" it for this frame)
            this._observer.UpdateState(gameTick);

            // Then, update all inputs that we're currently observing
            foreach (var keyState in this._keyStatesByKey.Select(x => x.Value))
            {
                this._observer.UpdateKey(keyState);
            }

            // Finally, execute the on-update reactions for each input
            foreach (var handler in this._handlersByExecutionOrder)
            {
                handler.Update(gameTick, _keyStatesByKey);
            }
        }

        #endregion

        #region Add/Remove KeyHandlers

        public Keycosystem AddHandler(KeyHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (this._keysByHandler.ContainsKey(handler))
            {
                throw new ArgumentException($"This handler has already been added!");
            }
            var keysForThisHandler = handler.GetAllHandledKeys().ToHashSet();
            this._keysByHandler.Add(handler, keysForThisHandler);
            
            foreach (var key in keysForThisHandler)
            {
                this.IndexHandlerByKey(handler, key);
            }

            this._handlersByExecutionOrder.Add(handler);
            handler.Keycosystem = this;
            return this;
        }

        public Keycosystem RemoveHandler(KeyHandler handler)
        {
            if (!this._keysByHandler.TryGetValue(handler, out var keysForThisHandler))
            {
                return this;
            }
            this._keysByHandler.Remove(handler);

            foreach (var key in keysForThisHandler)
            {
                this.DeindexHandlerByKey(handler, key);
            }

            this._handlersByExecutionOrder.Remove(handler);
            handler.Keycosystem = null;
            return this;
        }

        public Keycosystem ClearHandlers()
        {
            this._keyStatesByKey.Clear();
            this._handlersByKey.Clear();
            this._keysByHandler.Clear();
            this._handlersByExecutionOrder.Clear();

            return this;
        }

        #region Overloads

        public Keycosystem AddHandlers(IEnumerable<KeyHandler> handlers)
        {
            if (handlers == null || !handlers.Any())
            {
                throw new ArgumentException("There are no key handlers to add!");
            }

            foreach (var handler in handlers)
            {
                this.AddHandler(handler);
            }

            return this;
        }

        public Keycosystem AddHandlers(params KeyHandler[] handlers)
        {
            if (handlers == null || !handlers.Any())
            {
                throw new ArgumentException("There are no key handlers to add!");
            }

            foreach (var handler in handlers)
            {
                this.AddHandler(handler);
            }

            return this;
        }

        public Keycosystem RemoveHandlers(IEnumerable<KeyHandler> handlers)
        {
            if (handlers == null || !handlers.Any())
            {
                return this;
            }

            foreach (var handler in handlers)
            {
                this.RemoveHandler(handler);
            }

            return this;
        }

        public Keycosystem RemoveHandlers(params KeyHandler[] handlers)
        {
            if (handlers == null || !handlers.Any())
            {
                return this;
            }

            foreach (var handler in handlers)
            {
                this.RemoveHandler(handler);
            }

            return this;
        }

        #endregion

        #endregion

        #region Notifications (internal)

        internal void NotifyKeyBound(KeyHandler handler, int key)
        {
            this.IndexHandlerByKey(handler, key);

            // Index this key by this handler, so we can track every key that a given handler observes
            this._keysByHandler[handler].Add(key);
        }

        internal void NotifyKeyUnbound(KeyHandler handler, int key)
        {
            this.DeindexHandlerByKey(handler, key);

            // Remove this key from the handler index
            // Empty sets of keys are allowed as long as this handler hasn't been removed from the Keycosystem
            var keysByThisHandler = this._keysByHandler[handler];
            keysByThisHandler.Remove(key);
        }

        #endregion

        #region Private methods

        private void IndexHandlerByKey(KeyHandler handler, int key)
        {
            if (this._handlersByKey.TryGetValue(key, out var handlersByThisKey))
            {
                handlersByThisKey = new HashSet<KeyHandler>(ReferenceEqualityComparer<KeyHandler>.Instance);
                this._handlersByKey.Add(key, handlersByThisKey);

                // This key now has at least one handler, so it needs to be watched for updates
                this._keyStatesByKey.Add(key, new KeyState(key));
            }
            handlersByThisKey.Add(handler);
        }

        private void DeindexHandlerByKey(KeyHandler handler, int key)
        {
            var handlersByThisKey = this._handlersByKey[key];
            handlersByThisKey.Remove(handler);

            // If there are no handlers for this key anymore...
            if (!handlersByThisKey.Any())
            {
                // De-index this key completely
                this._handlersByKey.Remove(key);

                // We don't need to watch it for updates anymore
                this._keyStatesByKey.Remove(key);
            }
        }

        #endregion
    }
}
