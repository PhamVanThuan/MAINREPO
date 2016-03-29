using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Halo.Actions.Address;
using SAHL.Core.UI.Halo.Actions.LegalEntity;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.Address.Actions
{
    public class ChangeDomiciliumAddressActionConfiguration : ActionTileConfiguration<ChangeDomiciliumAddressActionTileModel>,
        IParentedActionTileConfiguration<LegalEntityAddressDrillDownTileConfiguration>
    {
        public ChangeDomiciliumAddressActionConfiguration()
            : base("ChangeDomiciliumAddressActionAccess", 1, UrlNames.EditorController, UrlNames.StartEditorAction, UrlAction.TileLaunchEditor)
        {
        }
    }
}