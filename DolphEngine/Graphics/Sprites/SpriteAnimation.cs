using System;
using System.Collections.Generic;

namespace DolphEngine.Graphics.Sprites
{
    public class SpriteAnimation
    {
        #region Constructors

        public SpriteAnimation(string name)
            : this(name, new List<Rect2d>(0))
        {
        }

        public SpriteAnimation(string name, IReadOnlyList<Rect2d> frames)
        {
            this.Name = name;
            this.Frames = frames;
        }

        public SpriteAnimation(string name, SpriteSheet spritesheet, IList<int> frames)
            : this(name, GetFramesFromSpritesheet(spritesheet, frames))
        {
        }

        #endregion

        #region Properties

        public readonly string Name;
        public readonly IReadOnlyList<Rect2d> Frames;

        public bool IsPlaying { get; protected set; }
        public TimeSpan StartTime { get; protected set; }
        public AnimationReplayMode ReplayMode { get; protected set; }
        public TimeSpan FrameDuration { get; protected set; }
        public TimeSpan TotalDuration { get; protected set; }

        #endregion

        #region Static methods

        public static List<Rect2d> GetFramesFromSpritesheet(SpriteSheet spritesheet, IList<int> frames)
        {
            var list = new List<Rect2d>(frames.Count);

            foreach (var frame in frames)
            {
                list.Add(spritesheet.Frames[frame]);
            }

            return list;
        }

        #endregion

        #region Public methods

        public void Play(TimeSpan duration, DurationMode mode, AnimationReplayMode replayMode)
        {
            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentException($"Cannot play an animation for zero or negative time.");
            }

            if (this.Frames.Count == 0)
            {
                throw new InvalidOperationException($"Cannot play an animation with no frames specified.");
            }

            this.IsPlaying = true;
            this.StartTime = GameTimer.Global.Total; // todo: kill this
            this.ReplayMode = replayMode;

            switch(mode)
            {
                case DurationMode.Frame:
                    this.FrameDuration = duration;
                    this.TotalDuration = duration * this.Frames.Count;
                    break;
                case DurationMode.Total:
                    this.TotalDuration = duration;
                    this.FrameDuration = duration / this.Frames.Count;
                    break;
                default:
                    throw new ArgumentException($"Unrecognized {nameof(DurationMode)}: {mode} ({mode:D})");
            }
        }

        public void Pause()
        {
            // I want this eventually!
            throw new NotImplementedException();
        }

        public void Stop()
        {
            this.IsPlaying = false;
            this.StartTime = TimeSpan.Zero;
            this.TotalDuration = TimeSpan.Zero;
            this.FrameDuration = TimeSpan.Zero;
        }

        public bool TryGetCurrentFrame(out Rect2d frame)
        {
            if (!this.IsPlaying || this.Frames.Count == 0)
            {
                frame = Rect2d.Zero;
                return false;
            }

            var currentTime = GameTimer.Global.Total; // todo: kill this
            var sequenceIndex = (int)((currentTime - this.StartTime) / this.FrameDuration);

            int sequenceIndexAdjusted;
            switch (this.ReplayMode)
            {
                case AnimationReplayMode.Loop:
                    // If you've gone past the last frame, start over at frame index 0
                    sequenceIndexAdjusted = sequenceIndex % this.Frames.Count;
                    break;
                case AnimationReplayMode.HoldOnLastFrame:
                    // If you've gone past the last frame, keep drawing the last frame
                    sequenceIndexAdjusted = Math.Min((int)(currentTime / this.FrameDuration), this.Frames.Count - 1);
                    break;
                case AnimationReplayMode.DisappearAfterLastFrame:
                    if (sequenceIndex > this.Frames.Count)
                    {
                        // If you've gone past the last frame, do not draw the sprite
                        frame = Rect2d.Zero;
                        return false;
                    }
                    sequenceIndexAdjusted = sequenceIndex;
                    break;
                default:
                    throw new InvalidOperationException($"Unrecognized {nameof(AnimationReplayMode)}: {this.ReplayMode}");
            }

            frame = this.Frames[sequenceIndexAdjusted];
            return true;
        }

        #endregion
    }

    public enum DurationMode
    {
        Frame,
        Total
    }

    public enum AnimationReplayMode
    {
        Loop,
        HoldOnLastFrame,
        DisappearAfterLastFrame
    }
}
