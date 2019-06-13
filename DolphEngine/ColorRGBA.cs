using System;
using System.Collections.Generic;
using System.Text;

namespace DolphEngine
{
    public struct ColorRGBA
    {
        #region Constructors

        public ColorRGBA(byte r, byte g, byte b) : this(r, g, b, 255) { }

        public ColorRGBA(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        #endregion

        #region Properties

        public byte R;

        public byte G;

        public byte B;

        public byte A;

        #endregion

        #region Derived properties

        public byte Hue => throw new System.NotImplementedException();

        public byte Sat => throw new System.NotImplementedException();

        public byte Lum => throw new System.NotImplementedException();

        public float Opacity => throw new System.NotImplementedException();

        #endregion
    }
}
