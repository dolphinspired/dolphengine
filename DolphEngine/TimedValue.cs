using System;
using System.Collections.Generic;
using System.Text;

namespace DolphEngine
{
    public class TimedValue
    {
        protected readonly GameTimer Timer;

        protected long LastChange;

        protected TimedValue(GameTimer timer)
        {
            this.Timer = timer;
        }

        public bool JustChanged => this.Timer.Total.Ticks == this.LastChange;
    }

    public class TimedValue<T> : TimedValue
    {
        public TimedValue(GameTimer timer) : this(timer, default(T)) { }

        public TimedValue(GameTimer timer, T def) : base(timer)
        {
            this._value = def;
        }

        private T _value;
        public T Value
        {
            get => this._value;
            set
            {
                this._value = value;
                this.LastChange = this.Timer.Total.Ticks;
            }
        }
    }
}
