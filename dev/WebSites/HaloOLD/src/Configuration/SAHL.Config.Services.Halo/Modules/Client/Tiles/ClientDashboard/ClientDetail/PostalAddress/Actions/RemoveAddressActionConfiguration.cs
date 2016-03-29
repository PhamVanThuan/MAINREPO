using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Halo.Actions.Address;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.PostalAddress.Actions
{
    public class RemoveAddressActionConfiguration : ActionTileConfiguration<RemoveAddressActionTileModel>,
                                                    IParentedActionTileConfiguration<LegalEntityPostalAddressDrillDownTileConfiguration>
    {
        public RemoveAddressActionConfiguration()
            : base("", 0, UrlNames.EditorController, UrlNames.StartEditorAction, UrlAction.TileLaunchEditor)
        {
        }
    }
}