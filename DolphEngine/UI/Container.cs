using DolphEngine.Graphics;
using DolphEngine.Graphics.Directives;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.UI
{
    public abstract class Container : IDirectiveChannel
    {
        public Rect2d Space;

        private List<Container> _children;
        public List<Container> Children
        {
            get => this._children ?? (this._children = new List<Container>(0));
            set => this._children = value;
        }

        public virtual IEnumerable<DrawDirective> Directives
        {
            get
            {
                yield break;
            }
        }

        public virtual IEnumerable<DrawDirective> TreeDirectives => Enumerable.Concat(this.Directives, this.Children.SelectMany(c => c.TreeDirectives));
    }
}
