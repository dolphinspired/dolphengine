using DolphEngine.Graphics.Animations;
using System;
using Xunit;

namespace DolphEngine.Test.Graphics
{
    public class AnimationTests
    {
        [Fact]
        public void CanAddKeyframe()
        {
            var anim = new KeyframeAnimation<Rect2d>();
            anim.AddKeyframe(TimeSpan.Zero, new Rect2d(1, 2, 3, 4));
            Assert.Single(anim.Keyframes);

            anim.AddKeyframe(TimeSpan.FromMilliseconds(30), new Rect2d(5, 6, 7, 8));
            Assert.Equal(2, anim.Keyframes.Count);
        }

        [Fact]
        public void CanOverwriteKeyframe()
        {
            var anim = new KeyframeAnimation<Rect2d>();
            anim.AddKeyframe(TimeSpan.FromMilliseconds(30), new Rect2d(1, 2, 3, 4));
            anim.AddKeyframe(TimeSpan.FromMilliseconds(30), new Rect2d(5, 6, 7, 8));
            Assert.Single(anim.Keyframes);
        }

        [Fact]
        public void CannotGetWithNoKeyframes()
        {
            var anim = new KeyframeAnimation<Rect2d>();
            Assert.Throws<InvalidOperationException>(() => anim.GetFrame(TimeSpan.FromMilliseconds(30)));
        }

        [Fact]
        public void CanGetExactSingleKeyframe()
        {
            var keyframe = "test";
            var time = TimeSpan.FromMinutes(5);

            var anim = new KeyframeAnimation<string>();            
            anim.AddKeyframe(time, keyframe);

            Assert.Equal(keyframe, anim.GetFrame(time));
        }

        [Fact]
        public void CanGetApproximateSingleKeyframe()
        {
            var keyframe = "test";
            var time = TimeSpan.FromMinutes(5);
            var diff = TimeSpan.FromSeconds(15);

            var anim = new KeyframeAnimation<string>();
            anim.AddKeyframe(time, keyframe);
            
            Assert.Equal(keyframe, anim.GetFrame(time.Subtract(diff)));
            Assert.Equal(keyframe, anim.GetFrame(time.Add(diff)));
        }

        [Fact]
        public void CanGetExactMultipleKeyframes()
        {
            var keyframe1 = "test-1";
            var keyframe2 = "test-2";
            var keyframe3 = "test-3";
            var time1 = TimeSpan.FromMinutes(5);
            var time2 = TimeSpan.FromMinutes(6);
            var time3 = TimeSpan.FromMinutes(7);

            var anim = new KeyframeAnimation<string>()
                .AddKeyframe(time1, keyframe1)
                .AddKeyframe(time2, keyframe2)
                .AddKeyframe(time3, keyframe3);
            
            Assert.Equal(keyframe1, anim.GetFrame(time1));
            Assert.Equal(keyframe2, anim.GetFrame(time2));
            Assert.Equal(keyframe3, anim.GetFrame(time3));
        }

        [Fact]
        public void CanGetApproximateMultipleKeyframes()
        {
            var keyframe1 = "test-1";
            var keyframe2 = "test-2";
            var keyframe3 = "test-3";
            var time1 = TimeSpan.FromMinutes(5);
            var time2 = TimeSpan.FromMinutes(6);
            var time3 = TimeSpan.FromMinutes(7);

            var anim = new KeyframeAnimation<string>()
                .AddKeyframe(time1, keyframe1)
                .AddKeyframe(time2, keyframe2)
                .AddKeyframe(time3, keyframe3);

            // Before 1st keyframe
            var mid01 = time1 - ((time2 - time1) / 2);
            Assert.True(mid01 < time1);
            Assert.Equal(keyframe1, anim.GetFrame(mid01));

            // Between keyframes 1 and 2
            var mid12 = time2 - ((time2 - time1) / 2);
            Assert.InRange(mid12, time1, time2);
            Assert.Equal(keyframe1, anim.GetFrame(mid12));

            // Between keyframes 2 and 3
            var mid23 = time2 + ((time3 - time2) / 2);
            Assert.InRange(mid23, time2, time3);
            Assert.Equal(keyframe2, anim.GetFrame(mid23));

            // After last keyframe
            var mid34 = time3 + ((time3 - time2) / 2);
            Assert.True(mid34 > time3);
            Assert.Equal(keyframe3, anim.GetFrame(mid34));
        }

        [Fact]
        public void CanLoopKeyframes()
        {
            var keyframe1 = "test-1";
            var keyframe2 = "test-2";
            var keyframe3 = "test-3";
            var time1 = TimeSpan.FromSeconds(2);
            var mid12 = TimeSpan.FromSeconds(10); // 7 seconds, loop, then 3 more seconds
            var time2 = TimeSpan.FromSeconds(4);
            var time3 = TimeSpan.FromSeconds(6);
            var loopDelay = TimeSpan.FromSeconds(1);

            var anim = new KeyframeAnimation<string>()
                .AddKeyframe(time1, keyframe1)
                .AddKeyframe(time2, keyframe2)
                .AddKeyframe(time3, keyframe3)
                .Loop(loopDelay);

            // Before 1st keyframe
            var mid01 = time1 - ((time2 - time1) / 2);
            Assert.True(mid01 < time1);
            Assert.Equal(keyframe3, anim.GetFrame(mid01));

            // After last keyframe, but before loop
            var mid34 = time3 + (loopDelay / 2);
            Assert.True(mid34 > time3);
            Assert.Equal(keyframe3, anim.GetFrame(mid34));

            // After one loop, then between frames 1 and 2
            Assert.Equal(keyframe1, anim.GetFrame(mid12));
        }

        [Theory]
        // Exact keyframe match
        [InlineData(false, false, 1000, 1000)]
        [InlineData(true , false, 2000, 2000)]
        [InlineData(false, true , 3000, 3000)]
        // Before 1st frame
        [InlineData(false, false, 250 , 1000)]
        [InlineData(true , false, 250 , 250 )] 
        [InlineData(false, true , 250 , 1750)] // 62.5% of the way from 3000 to 1000
        [InlineData(true , true , 250 , 250 )] // 25.0% of the way from 0 to 1000 (that's why the zero-frame is important!)
        // Between frames
        [InlineData(false, false, 1400, 1400)]
        [InlineData(true , false, 1950, 1950)]
        [InlineData(false, true , 2200, 2200)]
        [InlineData(true , true , 2865, 2865)]
        // After last frame
        [InlineData(false, false, 3500, 3000)]
        [InlineData(true , false, 3500, 3000)]
        [InlineData(false, true , 3500, 2500)] // 25.0% of the way from 3000 to 1000
        [InlineData(true , true , 3500, 1500)] // 50.0% of the way from 3000 to 0
        // After first iteration
        [InlineData(false, true , 4500, 1500)] // 75.0% of the way from 3000 to 1000
        [InlineData(false, true , 5250, 1250)] // 25.0% of the way from 1000 to 2000
        [InlineData(true , true , 4800, 800 )] // 80.0% of the way from 0 to 1000
        [InlineData(true , true , 6560, 2560)] // 56.0% of the way from 2000 to 3000
        public void CanTweenKeyframes(bool addZeroFrame, bool addLoop, int time, int expected)
        {
            var anim = new TestAnimation()
                .AddKeyframe(TimeSpan.FromMilliseconds(1000), 1000)
                .AddKeyframe(TimeSpan.FromMilliseconds(2000), 2000)
                .AddKeyframe(TimeSpan.FromMilliseconds(3000), 3000);

            if (addZeroFrame)
            {
                anim.AddKeyframe(TimeSpan.Zero, 0);
            }

            if (addLoop)
            {
                anim.Loop(TimeSpan.FromMilliseconds(1000));
            }

            var frame = anim.GetFrame(TimeSpan.FromMilliseconds(time));
            Assert.Equal(expected, frame);
        }

        private class TestAnimation : KeyframeAnimation<int>
        {
            public override int Tween(int prevFrame, int nextFrame, double elapsedRatio)
            {
                return prevFrame + (int)((nextFrame - prevFrame) * elapsedRatio);
            }
        }
    }
}
