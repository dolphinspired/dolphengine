using System;
using System.Linq;

namespace DolphEngine.Tools
{
    public class FpsCounter
    {
        private readonly long[] Samples;
        private int CurrentSample;

        public FpsCounter(int sampleSize)
        {
            this.Samples = new long[sampleSize];
        }

        public double Update()
        {
            this.CurrentSample = ++this.CurrentSample % Samples.Length;
            this.Samples[this.CurrentSample] = GameTimer.Global.Elapsed.Ticks;

            var frameAverage = Samples.Average();
            return (1 / frameAverage) * TimeSpan.TicksPerSecond;
        }
    }
}
