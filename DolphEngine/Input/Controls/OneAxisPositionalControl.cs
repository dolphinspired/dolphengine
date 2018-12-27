using System;

namespace DolphEngine.Input.Controls
{
    public class OneAxisPositionalControl : ControlBase
    {
        public OneAxisPositionalControl(string key)
        {
            this.SetKeys(key);
        }

        public override void Update()
        {
            this.LastPosition = this.Position;
            this.Position = this.InputState.GetValueOrDefault<int>(this.Keys[0]);

            if (this.Position != this.LastPosition)
            {
                this.LastTickMoved = this.InputState.CurrentTimestamp;
            }
        }

        private int LastPosition;
        public int Position { get; private set; }
        public long LastTickMoved { get; private set; }

        public int PositionDelta => this.Position - this.LastPosition;
        public int PositionDeltaAbsolute => Math.Abs(this.PositionDelta);
        public bool JustMoved => this.PositionDelta != 0;
        public long DurationHeld => InputState.CurrentTimestamp - LastTickMoved;
    }
}
