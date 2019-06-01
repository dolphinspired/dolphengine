using System.Collections.Generic;

namespace DolphEngine.Input
{
    public abstract class KeyStateObserver
    {
        public readonly InputState InputState = new InputState();
        private readonly Dictionary<string, int> _watchedKeys = new Dictionary<string, int>();
        
        /// <summary>
        /// Run once per frame.
        /// </summary>
        public abstract void UpdateState();
        
        /// <summary>
        /// Run once per key per frame.
        /// </summary>
        /// <param name="key">The generic key to look up</param>
        public abstract object GetKeyValue(string key);

        #region Non-public methods

        internal void Watch(ControlBase controller)
        {
            foreach (var key in controller.Keys)
            {
                if (this._watchedKeys.ContainsKey(key))
                {
                    this._watchedKeys[key]++;
                }
                else
                {
                    this._watchedKeys.Add(key, 1);
                }
            }
        }

        internal void Unwatch(ControlBase controller)
        {
            foreach (var key in controller.Keys)
            {
                if (this._watchedKeys.ContainsKey(key))
                {
                    this._watchedKeys[key]--;

                    if (this._watchedKeys[key] == 0)
                    {
                        this._watchedKeys.Remove(key);
                    }
                }
            }
        }

        internal void Update()
        {
            this.UpdateState();

            foreach (var kvp in this._watchedKeys)
            {
                this.InputState.SetValue(kvp.Key.ToString(), this.GetKeyValue(kvp.Key));
            }
        }

        #endregion
    }
}
