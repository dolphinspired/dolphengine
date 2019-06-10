using DolphEngine.MonoGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace DolphEngine.Demo
{
    public class FpsCounter
    {
        public SpriteFont Font;
        public Vector2d Position;
        
        public double Fps { get; private set; }
        
        private readonly SpriteBatch _spriteBatch;
        private readonly GameTimer _timer;
        
        private long[] _samples = new long[1];
        private int _currentFrame;
        private int _currentSample;

        public FpsCounter(SpriteBatch sb, GameTimer timer)
        {
            this._spriteBatch = sb;
            this._timer = timer;
        }

        public void SetSampleSize(int sampleSize)
        {
            this._samples = new long[sampleSize];
        }

        public void Update()
        {
            this._currentFrame++;
            this._currentSample = ++this._currentSample % this._samples.Length;
            this._samples[this._currentSample] = this._timer.Elapsed.Ticks;

            var frameAverage = this._samples.Sum() / (double)this._samples.Length;
            this.Fps = (1 / frameAverage) * TimeSpan.TicksPerSecond;
        }

        public void Draw()
        {
            var fpsText = this._currentFrame > _samples.Length ? $"{this.Fps:0.0}" : "--.-";

            this._spriteBatch.Begin();
            this._spriteBatch.DrawString(this.Font, $"FPS: {fpsText}", this.Position.ToVector2(), Color.White);
            this._spriteBatch.End();
        }
    }
}
