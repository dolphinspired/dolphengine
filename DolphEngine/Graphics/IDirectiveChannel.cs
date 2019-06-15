using DolphEngine.Graphics.Directives;
using System.Collections.Generic;

namespace DolphEngine.Graphics
{
    public interface IDirectiveChannel
    {
        IEnumerable<DrawDirective> Directives { get; }
    }
}
