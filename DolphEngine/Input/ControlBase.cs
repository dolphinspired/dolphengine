using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Input
{
    public abstract class ControlBase
    {
        public IReadOnlyCollection<string> Keys => this._keys;
        private readonly HashSet<string> _keys = new HashSet<string>();

        public IReadOnlyCollection<ControlBase> Controls => this._controls;
        private readonly List<ControlBase> _controls = new List<ControlBase>();

        private Keycosystem _keycosystem;
        protected InputState InputState => this._keycosystem.Observer.InputState;
        protected GameTimer Timer => this._keycosystem.Timer;

        #region Control building

        protected void AddKey(string key)
        {
            this._keys.Add(key);
        }

        protected T AddControl<T>(T control) where T : ControlBase
        {
            if (control.Keys.Count() == 0)
            {
                throw new ArgumentException($"The provided control of type {typeof(T).Name} has no keys!");
            }

            foreach (var key in control.Keys)
            {
                this.AddKey(key);
            }

            this._controls.Add(control);
            return control;
        }

        #endregion

        #region Lifecycle events

        public virtual void OnConnect() { }
        
        public void Connect(Keycosystem keycosystem)
        {
            if (this._keycosystem != null)
            {
                throw new InvalidOperationException($"This control of type {this.GetType().Name} is already connected to a {nameof(Keycosystem)}!");
            }

            foreach (var control in this._controls)
            {
                control.Connect(keycosystem);
            }

            this._keycosystem = keycosystem;
            this.OnConnect();
        }

        public virtual void OnUpdate() { }

        public void Update()
        {
            foreach (var control in this._controls)
            {
                control.Update();
            }

            this.OnUpdate();
        }

        public virtual void OnDisconnect() { }

        public void Disconnect()
        {
            foreach (var control in this._controls)
            {
                control.Disconnect();
            }

            this.OnDisconnect();
            this._keycosystem = null;
        }

        #endregion
    }
}
