using System;

namespace DolphEngine.Input.Controls
{
    public class OneAxisPositionalControl : ControlBase
    {
        private readonly string _key;

        public OneAxisPositionalControl(string key)
        {
            this._key = key;
            this.AddKey(key);
        }

        #region Event hooks

        public override void OnConnect()
        {
            this.LastTickMoved = this.Timer.Total.Ticks;
        }

        public override void OnUpdate()
        {
            this.LastPosition = this.Position;
            this.Position = this.InputState.GetValueOrDefault<int>(this._key);

            if (this.Position != this.LastPosition)
            {
                this.LastTickMoved = this.Timer.Total.Ticks;
            }
        }

        #endregion

        private int LastPosition;
        public int Position { get; private set; }
        public long LastTickMoved { get; private set; }

        public int PositionDelta => this.Position - this.LastPosition;
        public int PositionDeltaAbsolute => Math.Abs(this.PositionDelta);
        public bool JustMoved => this.PositionDelta != 0;
        public long DurationHeld => this.Timer.Total.Ticks - LastTickMoved;
    }
}
