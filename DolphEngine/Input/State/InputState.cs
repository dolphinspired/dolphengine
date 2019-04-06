using System;
using System.Collections.Generic;

namespace DolphEngine.Input.State
{
    public class InputState
    {
        public long CurrentTimestamp;

        private readonly Dictionary<string, object> _inputValuesByKey = new Dictionary<string, object>();

        public bool TryGetValue(string key, out object value)
        {
            if (this._inputValuesByKey.TryGetValue(key, out var v))
            {
                value = v;
                return true;
            }

            value = null;
            return false;
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            if (this._inputValuesByKey.TryGetValue(key, out var v))
            {
                try
                {
                    value = (T)v;
                    return true;
                }
                catch (InvalidCastException)
                {
                }
            }

            value = default(T);
            return false;
        }

        public object GetValueOrDefault(string key, object def = null)
        {
            return this.TryGetValue(key, out var value) ? value : def;
        }

        public T GetValueOrDefault<T>(string key, T def = default(T))
        {
            return this.TryGetValue<T>(key, out var value) ? value : def;
        }

        public void SetValue(string key, object value)
        {
            this._inputValuesByKey[key] = value;
        }

        public void RemoveValue(string key)
        {
            this._inputValuesByKey.Remove(key);
        }
    }
}
