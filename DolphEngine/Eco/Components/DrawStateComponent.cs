using System.Collections.Generic;

namespace DolphEngine.Eco.Components
{
    public class DrawStateComponent : Component
    {
        public int State;

        public Dictionary<int, int> FrameStates;

        public Dictionary<int, List<int>> SequenceStates;
    }
}
