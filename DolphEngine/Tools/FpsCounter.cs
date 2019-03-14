using System;
using System.Linq;

namespace DolphEngine.Tools
{
    public class FpsCounter
    {
        private readonly Func<long> Timer;
        private readonly long[] Samples;
        private long LastTick;
        private int CurrentSample;

        public FpsCounter(Func<long> timer, int sampleSize)
        {
            this.Timer = timer;
            this.Samples = new long[sampleSize];
        }

        public double Update()
        {
            var currentTick = this.Timer();
            this.CurrentSample = ++this.CurrentSample % Samples.Length;
            var measurement = currentTick - this.LastTick; // Measure the delta between this frame and the last one
            this.Samples[this.CurrentSample] = measurement; // Store that delta in the samples list
            this.LastTick = currentTick;

            var frameAverage = Samples.Average();
            return (1 / frameAverage) * TimeSpan.TicksPerSecond;
        }
    }
}
