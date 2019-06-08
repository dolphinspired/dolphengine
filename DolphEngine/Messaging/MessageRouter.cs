using System;
using System.Collections.Generic;

namespace DolphEngine.Messaging
{
    public class MessageRouter
    {
        private readonly Dictionary<string, object> _observedValues = new Dictionary<string, object>();
        private readonly Dictionary<string, object> _toUpdate = new Dictionary<string, object>();

        private readonly Dictionary<string, List<Action<object>>> _subscribers = new Dictionary<string, List<Action<object>>>();
        
        public MessageRouter()
        {

        }

        public MessageRouter Publish<TValue>(string channel, TValue value)
        {
            this._toUpdate[channel] = value;
            return this;
        }

        public MessageRouter Subscribe<TValue>(string channel, Action<TValue> reaction)
        {
            if (!this._subscribers.TryGetValue(channel, out var actions))
            {
                actions = new List<Action<object>>(1);
                this._subscribers.Add(channel, actions);
            }

            actions.Add(o => reaction((TValue)o));
            return this;
        }

        public void Update()
        {
            foreach (var kvp in this._toUpdate)
            {
                var channel = kvp.Key;
                this._observedValues[channel] = kvp.Value;

                if (this._subscribers.TryGetValue(channel, out var actions))
                {
                    foreach (var action in actions)
                    {
                        // Do something in response to a new value being published on this channel
                        action(kvp.Value);
                    }
                }
            }

            this._toUpdate.Clear();
        }
    }
}
