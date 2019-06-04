using System;
using System.Diagnostics;

namespace DolphEngine
{
    public interface IGameTimer
    {
        void Update();

        TimeSpan Elapsed { get; }

        TimeSpan Total { get; }

        int Frames { get; }
    }

    public class GameTimer : IGameTimer
    {
        protected readonly Stopwatch Stopwatch;
        private bool _started;

        public GameTimer()
        {
            this.Stopwatch = new Stopwatch();
        }

        public GameTimer(Stopwatch sw)
        {
            this.Stopwatch = sw;
        }

        public virtual void Update()
        {
            if (!_started)
            {
                this.Stopwatch.Start();
                this._started = true;
            }

            var totalElapsed = this.Stopwatch.Elapsed;

            this.Elapsed = totalElapsed - this.Total;
            this.Total = totalElapsed;
            this.Frames++;
        }

        public virtual TimeSpan Elapsed { get; private set; }

        public virtual TimeSpan Total { get; private set; }

        public virtual int Frames { get; private set; }
    }
}
