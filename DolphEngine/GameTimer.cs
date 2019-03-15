using System;

namespace DolphEngine
{
    public class GameTimer
    {
        public GameTimer() : this(TimeSpan.Zero) { }

        public GameTimer(TimeSpan start)
        {
            this.Elapsed = TimeSpan.Zero;
            this.Total = start;
        }

        public void Update(TimeSpan elapsed)
        {
            this.Elapsed = elapsed;
            this.Total += elapsed;
        }

        public TimeSpan Elapsed { get; private set; }

        public TimeSpan Total { get; private set; }
    }
}
