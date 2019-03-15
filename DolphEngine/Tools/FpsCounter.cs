using System;
using System.Linq;

namespace DolphEngine.Tools
{
    public class FpsCounter
    {
        private readonly GameTimer Timer;
        private readonly long[] Samples;
        private int CurrentSample;

        public FpsCounter(GameTimer timer, int sampleSize)
        {
            this.Timer = timer;
            this.Samples = new long[sampleSize];
        }

        public double Update()
        {
            this.CurrentSample = ++this.CurrentSample % Samples.Length;
            this.Samples[this.CurrentSample] = this.Timer.Elapsed.Ticks;

            var frameAverage = Samples.Average();
            return (1 / frameAverage) * TimeSpan.TicksPerSecond;
        }
    }
}
