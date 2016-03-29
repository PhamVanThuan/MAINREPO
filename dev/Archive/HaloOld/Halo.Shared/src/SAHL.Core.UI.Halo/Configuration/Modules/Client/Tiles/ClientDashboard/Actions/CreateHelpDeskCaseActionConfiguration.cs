using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Tiles;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Halo.Actions.LegalEntity;

namespace SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.Actions
{
    public class CreateHelpDeskCaseActionConfiguration : ActionTileConfiguration<CreateHelpDeskCaseActionTileModel>, IParentedActionTileConfiguration<LegalEntityRootTileConfiguration>
    {
        public CreateHelpDeskCaseActionConfiguration()
            : base("CreateHelpDeskCaseActionAccess", 0, UrlNames.EditorController, UrlNames.StartEditorAction, UrlAction.TileLaunchEditor)
        {
        }
    }
}