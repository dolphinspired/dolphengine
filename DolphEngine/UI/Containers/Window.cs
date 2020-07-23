using System.Collections.Generic;
using System.Linq;
using DolphEngine.Graphics.Directives;

namespace DolphEngine.UI.Containers
{
    public class Window : UiElement
    {
        public override IEnumerable<DrawDirective> Directives => this.Children.SelectMany(c => c.TreeDirectives);
    }
}
