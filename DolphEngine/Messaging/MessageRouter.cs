using System;
using System.Collections.Generic;

namespace DolphEngine.Messaging
{
    public class MessageRouter
    {
        // todo: possible to clean up unused channels?
        private readonly Dictionary<string, MessageChannel> _channels = new Dictionary<string, MessageChannel>();
        private readonly List<Action> _toPublish = new List<Action>();
        
        public PubKey<TValue> GetPubKey<TValue>(string channelName)
        {
            // Benefit of publishing through a key is that runtime type checking can happen just once, when 
            // the key is created, rather than every time you publish a value.
            var channel = this.GetOrCreateChannel<TValue>(channelName);
            return new PubKey<TValue>(this, channel, channelName);
        }

        public SubKey GetSubKey()
        {
            return new SubKey(this);
        }

        public void Update()
        {
            if (this._toPublish.Count > 0)
            {
                foreach (var publishAction in this._toPublish)
                {
                    publishAction();
                }

                this._toPublish.Clear();
            }
        }

        #region Non-public methods (Pub/Sub)

        internal void Publish<TValue>(PubKey<TValue> pk, TValue value)
        {
            // Delegate publication to later, will publish during update loop
            this._toPublish.Add(() => pk.Channel.Publish(value));
        }

        internal void Subscribe<TValue>(SubKey sk, string channelName, Action<TValue> handler)
        {
            var channel = this.GetOrCreateChannel<TValue>(channelName);
            channel.Subscribe(sk, handler);
        }

        internal void Unsubscribe(SubKey sk, string channelName)
        {
            if (this._channels.TryGetValue(channelName, out var channel))
            {
                channel.Unsubscribe(sk);
            }
        }

        internal void Unsubscribe(SubKey sk)
        {
            foreach (var channel in this._channels.Values)
            {
                channel.Unsubscribe(sk);
            }
        }

        #endregion

        #region Non-public methods (other)

        private MessageChannel<TValue> GetOrCreateChannel<TValue>(string channelName)
        {
            MessageChannel<TValue> typedChannel;

            if (!this._channels.TryGetValue(channelName, out var channel))
            {
                // If the no channel exists yet, create one
                typedChannel = new MessageChannel<TValue>();
                this._channels.Add(channelName, typedChannel);
            }
            else
            {
                // Otherwise, use the existing channel
                if (channel.Type != typeof(TValue))
                {
                    throw new InvalidCastException($"Cannot publish value of type '{typeof(TValue).Name}' to channel '{channelName}'," +
                        $" which expects type '{channel.Type.Name}'!");
                }

                typedChannel = (MessageChannel<TValue>)channel;
            }

            return typedChannel;
        }

        #endregion
    }
}
