using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Context;
using SAHL.Core.UI.Elements.Tiles;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Configuration.Tiles
{
    public abstract class ActionTileConfiguration<T> : AbstractTileConfiguration<T>, IActionTileConfiguration where T : IActionTileModel
    {
        public ActionTileConfiguration(string requiredFeatureAccess, int sequence, string controllerName, string actionName, UrlAction urlAction)
            : base(requiredFeatureAccess, sequence, controllerName, actionName, urlAction)
        {
        }

        public ActionMiniTileElement CreateElement(BusinessContext businessContext)
        {
            return new ActionMiniTileElement(new TileBusinessContext(businessContext, this.TileModelType, this.GetType()), this.Url.GenerateActionUrl(), this.Url.UrlAction);
        }
    }
}