using SAHL.Core.UI.Context;
using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Elements.Tiles
{
    public class MajorTileElement : AbstractTileElement
    {
        public MajorTileElement(TileBusinessContext businessContext, string url)
            : base(businessContext, url, UrlAction.TileDrillDown)
        {
        }
    }
}