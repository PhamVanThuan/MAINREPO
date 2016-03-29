using System.Collections.Generic;

namespace SAHL.Core.UI.Elements
{
    public interface IVisualElement : IElement
    {
        IEnumerable<Part> Parts { get; }

        void AddPart(Part part);

        void InsertPartBefore(Part partToAdd, Part beforeThisPart);
    }
}