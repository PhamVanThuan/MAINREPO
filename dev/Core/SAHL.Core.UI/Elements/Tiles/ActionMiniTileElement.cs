using SAHL.Core.UI.Context;
using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Elements.Tiles
{
    public class ActionMiniTileElement : MiniTileElement
    {
        public ActionMiniTileElement(TileBusinessContext businessContext, string url, UrlAction urlAction)
            : base(businessContext, url, urlAction, TileSize.Size_1_by_1)
        {
        }
    }
}