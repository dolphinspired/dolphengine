using DolphEngine.Eco;
using System;

namespace DolphEngine.Demo.Components
{
    public class SelectableItemComponent : Component
    {
        public bool Selected;

        public Action OnFocus;

        public Action OnBlur;
    }
}
