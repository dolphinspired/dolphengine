namespace DolphEngine.Messaging
{
    /// <summary>
    /// Allows publication of type-verified values to a single <see cref="MessageChannel"/> via the linked
    /// <see cref="MessageRouter"/>. Avoids casting values on each publish.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class PubKey<TValue>
    {
        private readonly MessageRouter _router;
        internal readonly MessageChannel<TValue> Channel;
        public readonly string ChannelName;

        public PubKey(MessageRouter router, MessageChannel<TValue> channel, string channelName)
        {
            this._router = router;
            this.Channel = channel;
            this.ChannelName = channelName;
        }
        
        public PubKey<TValue> Publish(TValue value)
        {
            this._router.Publish(this, value);
            return this;
        }
    }
}
