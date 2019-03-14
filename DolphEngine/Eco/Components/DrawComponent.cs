using System.Collections.Generic;

namespace DolphEngine.Eco.Components
{
    public class DrawComponent : Component
    {
        public List<object> Directives
        {
            get => this._directives ?? (this._directives = new List<object>(0));
            set => this._directives = value;
        }
        private List<object> _directives;
    }
}
