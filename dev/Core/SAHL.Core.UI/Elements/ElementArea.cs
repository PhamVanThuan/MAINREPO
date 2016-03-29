using System.Collections.Generic;

namespace SAHL.Core.UI.Elements
{
    public class ElementArea : Element, IElementArea
    {
        private List<Element> elements;

        public ElementArea(string id)
            : base(id)
        {
            this.elements = new List<Element>();
        }

        public IEnumerable<Element> Elements { get { return this.elements; } }

        protected void AddElement(Element element)
        {
            this.elements.Add(element);
        }
    }
}