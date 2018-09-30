using System.Collections.Generic;
using System.Linq;

namespace TacticsGame.Engine
{
    public static class AnimationSchedules
    {
        public static readonly AnimationSchedule WalkCycle = new AnimationSchedule(150, 0, 1, 1, 1, 0, 2, 2, 2);
    }

    public class AnimationSchedule
    {
        public AnimationSchedule(uint durationMs, params uint[] pattern)
        {
            this.Pattern = pattern.Select(x => new AnimationFrame(durationMs, x)).ToArray();
        }

        public AnimationSchedule(IReadOnlyList<AnimationFrame> pattern)
        {
            this.Pattern = pattern;
        }

        public readonly IReadOnlyList<AnimationFrame> Pattern;

        public override string ToString()
        {
            return string.Join(",", this.Pattern.Select(x => $"({x})"));
        }
    }

    public struct AnimationFrame
    {
        public AnimationFrame(uint durationMs, uint frame)
        {
            this.DurationMs = durationMs;
            this.Frame = frame;
        }

        public readonly uint DurationMs;

        public readonly uint Frame;

        public override string ToString()
        {
            return $"{this.Frame}:{this.DurationMs}ms";
        }
    }
}