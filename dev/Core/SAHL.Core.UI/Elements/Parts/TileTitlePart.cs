using System.Collections.Generic;

namespace SAHL.Core.UI.Elements.Parts
{
    public class TileTitlePart : Part
    {
        private List<Part> parts;

        public TileTitlePart()
        {
            this.parts = new List<Part>();
        }

        public IEnumerable<Part> Parts { get { return this.parts; } }

        public void AddPart(Part part)
        {
            this.parts.Add(part);
        }
    }
}