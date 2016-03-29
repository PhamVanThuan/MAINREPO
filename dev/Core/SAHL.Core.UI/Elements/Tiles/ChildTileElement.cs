using SAHL.Core.UI.Context;
using SAHL.Core.UI.Enums;

namespace SAHL.Core.UI.Elements.Tiles
{
    public class ChildTileElement : AbstractTileElement
    {
        public ChildTileElement(TileBusinessContext businessContext, string url, UrlAction urlAction, TileSize tileSize)
            : base(businessContext, url, urlAction)
        {
            this.TileSize = tileSize;
        }

        public TileSize TileSize { get; protected set; }
    }
}