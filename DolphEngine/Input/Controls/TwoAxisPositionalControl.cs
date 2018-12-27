﻿using System;
using DolphEngine.Input.State;

namespace DolphEngine.Input.Controls
{
    public class TwoAxisPositionalControl : ControlBase
    {
        public TwoAxisPositionalControl(string xKey, string yKey)
        {
            this.X = new OneAxisPositionalControl(xKey);
            this.Y = new OneAxisPositionalControl(yKey);
            this.SetKeys(xKey, yKey);
        }

        public override void SetInputState(InputState inputState)
        {
            base.SetInputState(inputState);
            this.X.SetInputState(inputState);
            this.Y.SetInputState(inputState);
        }

        public override void Update()
        {
            this.X.Update();
            this.Y.Update();

            this.Direction = Direction.None;

            if (this.X.JustMoved || this.Y.JustMoved)
            {
                this.LastTickMoved = Math.Max(this.X.LastTickMoved, this.Y.LastTickMoved);

                if (this.X.PositionDelta > 0)
                {
                    this.Direction |= Direction.Right;
                }
                else if (this.X.PositionDelta < 0)
                {
                    this.Direction |= Direction.Left;
                }

                if (this.Y.PositionDelta > 0)
                {
                    this.Direction |= Direction.Down;
                }
                else if (this.Y.PositionDelta < 0)
                {
                    this.Direction |= Direction.Up;
                }
            }
        }

        public readonly OneAxisPositionalControl X;
        public readonly OneAxisPositionalControl Y;

        public Direction Direction { get; private set; }
        public long LastTickMoved { get; private set; } 

        public bool JustMoved => DurationHeld == 0;
        public long DurationHeld => InputState.CurrentTimestamp - LastTickMoved;
    }
}