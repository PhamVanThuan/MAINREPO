using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Elements.Tiles;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Configuration.Tiles
{
    public abstract class MiniTileConfiguration<T> : AbstractTileConfiguration<T>, IChildTileConfiguration<T> where T : ITileModel
    {
        public MiniTileConfiguration(string requiredFeatureAccess, int sequence, string controllerName, string controllerAction, UrlAction urlAction, TileSize tileSize = TileSize.Size_1_by_1)
            : base(requiredFeatureAccess, sequence, controllerName, controllerAction, urlAction)
        {
            this.TileSize = tileSize;
        }

        public MiniTileConfiguration(string requiredFeatureAccess, int sequence, TileSize tileSize = TileSize.Size_1_by_1)
            : base(requiredFeatureAccess, sequence)
        {
            this.TileSize = tileSize;
        }

        public TileSize TileSize { get; protected set; }

        public ChildTileElement CreateElement(BusinessContext businessContext)
        {
            return new MiniTileElement(new TileBusinessContext(businessContext, this.TileModelType, this.GetType()), this.Url.GenerateActionUrl(), this.Url.UrlAction, this.TileSize);
        }
    }
}