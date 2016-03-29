using SAHL.Core.BusinessModel;
using System.Collections.Generic;

namespace SAHL.Core.UI.Elements
{
    public class VisualElement : Element, IVisualElement
    {
        private List<Part> parts;

        public VisualElement(string id)
            : base(id)
        {
            this.parts = new List<Part>();
        }

        public VisualElement(string idPrefix, BusinessContext businessContext)
            : this(string.Format("{0}_{1}_{2}", idPrefix, businessContext.BusinessKey.KeyType, businessContext.BusinessKey.Key))
        {
            this.BusinessContext = businessContext;
        }

        public IEnumerable<Part> Parts { get { return this.parts; } }

        public void AddPart(Part part)
        {
            this.parts.Add(part);
        }

        public void InsertPartBefore(Part partToAdd, Part beforeThisPart)
        {
            int currentIndex = this.parts.IndexOf(beforeThisPart);
            if (currentIndex >= 0)
            {
                this.parts.Insert(currentIndex, partToAdd);
            }
        }
    }
}