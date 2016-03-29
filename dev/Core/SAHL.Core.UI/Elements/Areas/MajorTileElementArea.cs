using SAHL.Core.UI.Elements.Tiles;

namespace SAHL.Core.UI.Elements.Areas
{
    public class MajorTileElementArea : ElementArea
    {
        public MajorTileElementArea()
            : base(ElementNames.MajorTileArea)
        {
        }

        public MajorTileElement MajorTile { get; set; }
    }
}