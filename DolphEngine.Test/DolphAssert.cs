﻿using System;
using Xunit;

namespace DolphEngine.Test
{
    public class DolphAssert
    {
        public static void EqualF(float expected, float actual)
        {
            Assert.True(Math.Abs(expected - actual) < Constants.FloatTolerance, $"{actual} varies from {expected} by {expected - actual:F6}");
        }

        #region Object overrides

        [Obsolete("Do not call this method!", true)]
        public static new bool Equals(object o1, object o2)
        {
            throw new InvalidOperationException("Do not call this method!");
        }

        [Obsolete("Do not call this method!", true)]
        public static new bool ReferenceEquals(object o1, object o2)
        {
            throw new InvalidOperationException("Do not call this method!");
        }

        #endregion
    }
}
