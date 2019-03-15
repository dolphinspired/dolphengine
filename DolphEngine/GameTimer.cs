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

        public static readonly GameTimer Global = new GameTimer();

        public void Advance(TimeSpan elapsed)
        {
            this.Elapsed = elapsed;
            this.Total += elapsed;
            this.Frames++;
        }

        public TimeSpan Elapsed { get; private set; }

        public TimeSpan Total { get; private set; }

        public int Frames { get; private set; }
    }
}
