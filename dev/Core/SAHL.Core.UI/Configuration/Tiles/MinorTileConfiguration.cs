using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Elements.Tiles;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Configuration.Tiles
{
    public abstract class MinorTileConfiguration<T> : AbstractTileConfiguration<T>, IChildTileConfiguration<T> where T : ITileModel
    {
        public MinorTileConfiguration(string requiredFeatureAccess, int sequence, string controllerName, string controllerAction, UrlAction urlAction, TileSize tileSize = Enums.TileSize.Size_2_by_2)
            : base(requiredFeatureAccess, sequence, controllerName, controllerAction, urlAction)
        {
            this.TileSize = tileSize;
        }

        public MinorTileConfiguration(string requiredFeatureAccess, int sequence, TileSize tileSize = Enums.TileSize.Size_2_by_2)
            : base(requiredFeatureAccess, sequence)
        {
            this.TileSize = tileSize;
        }

        public TileSize TileSize { get; protected set; }

        public ChildTileElement CreateElement(BusinessContext businessContext)
        {
            return new MinorTileElement(new TileBusinessContext(businessContext, this.TileModelType, this.GetType()), this.Url.GenerateActionUrl(), this.Url.UrlAction, this.TileSize);
        }
    }
}