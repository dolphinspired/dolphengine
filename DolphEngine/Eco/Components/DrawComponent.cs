using DolphEngine.Graphics.Directives;
using System.Collections.Generic;

namespace DolphEngine.Eco.Components
{
    public class DrawComponent : Component
    {
        public List<DrawDirective> Directives
        {
            get => this._directives ?? (this._directives = new List<DrawDirective>(0));
            set => this._directives = value;
        }
        private List<DrawDirective> _directives;
    }
}
