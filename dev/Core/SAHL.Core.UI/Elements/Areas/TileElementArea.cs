using Newtonsoft.Json;
using System.Linq;

namespace SAHL.Core.UI.Elements.Areas
{
    public class TileElementArea : ElementArea
    {
        public TileElementArea()
            : base(ElementNames.TileArea)
        {
            this.AddElement(new MajorTileElementArea());
            this.AddElement(new ChildTileElementArea());
            this.AddElement(new ActionTileArea());
        }

        [JsonConstructor]
        public TileElementArea(string nondefault)
            : base(ElementNames.TileArea)
        {
        }

        public MajorTileElementArea MajorTileArea { get { return this.Elements.OfType<MajorTileElementArea>().Single(); } }

        public ChildTileElementArea ChildTileArea { get { return this.Elements.OfType<ChildTileElementArea>().Single(); } }

        public ActionTileArea ActionTileArea { get { return this.Elements.OfType<ActionTileArea>().Single(); } }
    }
}