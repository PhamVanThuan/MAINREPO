using System.Linq;

namespace SAHL.Core.UI.Elements.Areas
{
    public class TiledApplicationElementArea : ApplicationElementArea
    {
        public TiledApplicationElementArea()
            : base(ElementNames.TiledApplicationArea)
        {
            this.AddElement(new TileElementArea());
            this.AddElement(new SummaryElementArea());
        }

        public TileElementArea TileArea { get { return this.Elements.OfType<TileElementArea>().Single(); } }

        public SummaryElementArea SummaryArea { get { return this.Elements.OfType<SummaryElementArea>().Single(); } }
    }
}