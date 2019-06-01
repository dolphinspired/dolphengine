using System;

namespace DolphEngine.Input.Controls
{
    public class TwoAxisPositionalControl : ControlBase
    {
        public TwoAxisPositionalControl(string xKey, string yKey)
        {
            this.X = this.AddControl(new OneAxisPositionalControl(xKey));
            this.Y = this.AddControl(new OneAxisPositionalControl(yKey));
        }

        #region Event hooks

        public override void OnConnect()
        {
            this.LastTickMoved = this.Timer.Total.Ticks;
        }

        public override void OnUpdate()
        {
            this.Direction = Direction2d.None;

            if (this.X.JustMoved || this.Y.JustMoved)
            {
                this.LastTickMoved = Math.Max(this.X.LastTickMoved, this.Y.LastTickMoved);

                if (this.X.PositionDelta > 0)
                {
                    this.Direction |= Direction2d.Right;
                }
                else if (this.X.PositionDelta < 0)
                {
                    this.Direction |= Direction2d.Left;
                }

                if (this.Y.PositionDelta > 0)
                {
                    this.Direction |= Direction2d.Down;
                }
                else if (this.Y.PositionDelta < 0)
                {
                    this.Direction |= Direction2d.Up;
                }
            }
        }

        #endregion

        public readonly OneAxisPositionalControl X;
        public readonly OneAxisPositionalControl Y;

        public Direction2d Direction { get; private set; }
        public long LastTickMoved { get; private set; } 

        public bool JustMoved => DurationHeld == 0;
        public long DurationHeld => this.Timer.Total.Ticks - LastTickMoved;
    }
}
