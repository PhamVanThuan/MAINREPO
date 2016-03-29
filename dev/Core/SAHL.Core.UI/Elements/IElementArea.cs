using System.Collections.Generic;

namespace SAHL.Core.UI.Elements
{
    public interface IElementArea : IElement
    {
        IEnumerable<Element> Elements { get; }
    }
}