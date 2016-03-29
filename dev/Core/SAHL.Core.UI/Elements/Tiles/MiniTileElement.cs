using SAHL.Core.UI.Context;
using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Elements.Tiles
{
    public class MiniTileElement : ChildTileElement
    {
        public MiniTileElement(TileBusinessContext businessContext, string url, UrlAction urlAction, TileSize tileSize)
            : base(businessContext, url, urlAction, tileSize)
        {
        }
    }
}