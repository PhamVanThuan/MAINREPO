using Newtonsoft.Json;
using SAHL.Core.UI.Elements.Tiles;
using System.Collections.Generic;

namespace SAHL.Core.UI.Elements.Areas
{
    public class ChildTileElementArea : ElementArea
    {
        [JsonProperty]
        private List<ChildTileElement> childElements;

        public ChildTileElementArea()
            : base(ElementNames.ChildTileArea)
        {
            this.childElements = new List<ChildTileElement>();
        }

        public IEnumerable<ChildTileElement> ChildTiles { get { return this.childElements; } }

        public void AddChildElements(IEnumerable<ChildTileElement> childElements)
        {
            this.childElements.Clear();
            this.childElements.AddRange(childElements);
        }
    }
}