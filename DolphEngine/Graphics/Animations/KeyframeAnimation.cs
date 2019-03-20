using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Graphics.Animations
{
    public class KeyframeAnimation<T>
    {
        public IReadOnlyDictionary<long, T> Keyframes => this._keyframes;
        public TimeSpan Duration => new TimeSpan(_finalKeyframe + _loopbackDelay ?? 0);

        private readonly SortedList<long, T> _keyframes = new SortedList<long, T>();
        private static readonly Comparer<long> Comparer = Comparer<long>.Default;
        private long? _loopbackDelay;
        private long _finalKeyframe;

        #region Constructors

        public KeyframeAnimation()
        {
        }

        public KeyframeAnimation(T keyframe)
        {
            this.AddKeyframe(TimeSpan.Zero, keyframe);
        }

        public KeyframeAnimation(TimeSpan interval, bool loop, IEnumerable<T> keyframes)
        {
            if (keyframes == null || keyframes.Count() == 0)
            {
                throw new ArgumentException($"At least one keyframe is required!");
            }

            var time = TimeSpan.Zero;
            foreach (var keyframe in keyframes)
            {
                this.AddKeyframe(time, keyframe);
                time += interval;
            }

            if (loop)
            {
                this.Loop(interval);
            }
        }

        #endregion

        #region Public methods

        public KeyframeAnimation<T> AddKeyframe(TimeSpan time, T keyframe)
        {
            if (time < TimeSpan.Zero)
            {
                throw new ArgumentException($"Keyframe time cannot be less than zero!");
            }

            var tick = time.Ticks;
            if (this._keyframes.ContainsKey(tick))
            {
                this._keyframes[tick] = keyframe;
            }
            else
            {
                this._keyframes.Add(tick, keyframe);

                if (tick > _finalKeyframe)
                {
                    _finalKeyframe = tick;
                }
            }

            return this;
        }

        public KeyframeAnimation<T> Loop()
        {
            return this.Loop(TimeSpan.Zero);
        }

        public KeyframeAnimation<T> Loop(TimeSpan time)
        {
            this._loopbackDelay = time.Ticks;
            return this;
        }

        public T GetFrame(TimeSpan elapsed)
        {
            if (this.TryGetKeyframe(elapsed, out var prevKvp, out var nextKvp))
            {
                // If an exact keyframe was found, then prev/next are the same
                return prevKvp.Value;
            }

            // Calculate how far we are between the two keyframes, then use that value in tweening
            double currentTime = elapsed.Ticks;
            double prevTime = prevKvp.Key;
            double nextTime = nextKvp.Key;
            double elapsedRatio;

            if (currentTime > nextTime)
            {
                var x = 1 == 1;
            }

            if (this._loopbackDelay.HasValue)
            {
                currentTime = currentTime % (_finalKeyframe + _loopbackDelay.Value);
            }

            if (prevTime > nextTime)
            {
                // Edge case for looping, where the "earlier" time might actually be later than the "later" time, or vice versa
                // In this instance, calculate the gap between the last and first frame, plus the loopback delay
                double gap = _loopbackDelay.Value + nextTime; // nextTime == first frame

                if (currentTime > prevTime)
                {
                    // After last frame, but during loopback delay
                    elapsedRatio = (currentTime - prevTime) / gap;
                }
                else // currentTime must be less than nextTime
                {
                    // Before first frame, but after loopback delay
                    elapsedRatio = (currentTime + _loopbackDelay.Value) / gap;
                }
            }
            else
            {
                elapsedRatio = (currentTime - prevTime) / (nextTime - prevTime);
            }

            return this.Tween(prevKvp.Value, nextKvp.Value, elapsedRatio);
        }

        /// <summary>
        /// Override this method to specify what should happen when an exact keyframe cannot be resolved.
        /// This can prefer either the lower or higher frame, or create a new frame that's an interpolation between the two.
        /// By default, this simply returns the earlier frame in the animation.
        /// </summary>
        /// <param name="prevKeyframe">The earlier of the two keyframes in the animation</param>
        /// <param name="nextKeyframe">The later of the two keyframes in the animation</param>
        /// <param name="elapsedRatio">Between 0.0 and 1.0, how much time has elapsed between the two frames</param>
        /// <remarks>
        /// For example, if the earlier frame occurs at 200ms, the later frame at 400ms, and the current elapsed time is 350ms,
        /// then it would be 75% of the way between the frames, or elapsedRatio = 0.75
        /// </remarks>
        public virtual T Tween(T prevKeyframe, T nextKeyframe, double elapsedRatio)
        {
            return prevKeyframe;
        }

        #endregion

        #region Non-public methods

        private bool TryGetKeyframe(TimeSpan elapsed, out KeyValuePair<long, T> prev, out KeyValuePair<long, T> next)
        {
            if (this._keyframes.Count == 0)
            {
                throw new InvalidOperationException($"This animation has no keyframes!");
            }

            var tick = elapsed.Ticks;

            if (this._loopbackDelay.HasValue)
            {
                // The animation's duration is the highest added time value + the loopback delay
                tick = tick % (_finalKeyframe + _loopbackDelay.Value);
            }

            if (this._keyframes.TryGetValue(tick, out var keyframe))
            {
                // Exact match
                prev = new KeyValuePair<long, T>(tick, keyframe);
                next = prev;
                return true;
            }

            if (this._keyframes.Count == 1)
            {
                prev = this._keyframes.Single();
                next = prev;
                return true;
            }

            // Perform a binary search to find the nearest prev/next keys
            // adapted from: https://stackoverflow.com/a/594528
            var keys = this._keyframes.Keys;
            int min = 0, max = keys.Count - 1;

            int lo = min, hi = max;
            while (lo < hi)
            {
                var mid = (lo + hi) / 2;
                if (Comparer.Compare(keys[mid], tick) < 0)
                {
                    lo = mid + 1;
                }
                else
                {
                    hi = mid - 1;
                }
            }
            if (Comparer.Compare(keys[lo], tick) < 0)
            {
                lo++;
            }

            var prevIndex = Math.Max(min, lo - 1);
            var nextIndex = Math.Min(max, lo);

            if (this._loopbackDelay.HasValue && prevIndex == nextIndex)
            {
                // If the frames match at this point, we are either before the 1st or after the last frame
                // If looping is enabled, we need to "link the two ends together"
                if (prevIndex == min)
                {
                    prevIndex = max;
                }
                else if (nextIndex == max)
                {
                    nextIndex = min;
                }
            }

            var prevKey = keys[prevIndex];
            var nextKey = keys[nextIndex];
            prev = new KeyValuePair<long, T>(prevKey, this._keyframes[prevKey]);
            next = new KeyValuePair<long, T>(nextKey, this._keyframes[nextKey]);
            return prevIndex == nextIndex; // An exact match was found if both indices are the same
        }

        #endregion
        
        #region Object overrides

        public override string ToString()
        {
            return $"{{ frames: {_keyframes.Count}, duration: \"{Duration.TotalMilliseconds}ms\" }}";
        }

        #endregion
    }
}
