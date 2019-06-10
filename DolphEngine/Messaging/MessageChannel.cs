using System;
using System.Collections.Generic;

namespace DolphEngine.Messaging
{
    public abstract class MessageChannel
    {
        public MessageChannel(Type type)
        {
            this.Type = type;
        }

        public readonly Type Type;

        public abstract void Unsubscribe(SubKey sk);
    }

    public class MessageChannel<T> : MessageChannel
    {   
        private readonly Dictionary<SubKey, Action<T>> _subscriptions 
            = new Dictionary<SubKey, Action<T>>(ReferenceEqualityComparer<SubKey>.Instance);

        public MessageChannel() : base(typeof(T)) { }

        public void Publish(T value)
        {
            foreach (var reaction in this._subscriptions.Values)
            {
                reaction(value);
            }
        }

        public void Subscribe(SubKey sk, Action<T> handler)
        {
            if (this._subscriptions.TryGetValue(sk, out var existingHandler))
            {
                // Multiple actions can be supported by nesting the existing action inside the current one
                // Is this necessary? Probably not. Other options: 1) throw an exception. 2) maintain a list of actions.
                this._subscriptions[sk] = v => { handler(v); existingHandler(v); };
            }
            else
            {
                this._subscriptions[sk] = handler;
            }
        }

        public override void Unsubscribe(SubKey sk)
        {
            this._subscriptions.Remove(sk);
        }
    }
}
