using System;

namespace DolphEngine.Messaging
{
    /// <summary>
    /// Allows you to subscribe handlers to any channels managed by a <see cref="MessageRouter"/>.
    /// If a channel does not yet exist upon subscription, it will be created.
    /// </summary>
    public class SubKey : IDisposable
    {
        private readonly MessageRouter _router;

        public SubKey(MessageRouter router)
        {
            this._router = router;
        }

        public SubKey Subscribe<TValue>(string channel, Action<TValue> handler)
        {
            this._router.Subscribe(this, channel, handler);
            return this;
        }

        public SubKey Unsubscribe(string channel)
        {
            this._router.Unsubscribe(this, channel);
            return this;
        }

        public SubKey UnsubscribeAll()
        {
            this._router.Unsubscribe(this);
            return this;
        }

        #region Disposal

        // Following the pattern suggested here: https://stackoverflow.com/a/151244

        ~SubKey()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                // Not actually using the disposing variable at this time

                this._router.Unsubscribe(this);
                this._disposed = true;
            }
        }

        #endregion
    }
}
