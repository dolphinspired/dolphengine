using DolphEngine.Eco;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DolphEngine.MonoGame.Eco.Components
{
    public class DrawComponent : Component
    {
        public readonly List<Action<SpriteBatch>> DrawDelegates = new List<Action<SpriteBatch>>();
    }
}
