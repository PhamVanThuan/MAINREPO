using SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.ResidentialAddress.Actions;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Editors;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Halo.Editors.Address.RemoveAddressEditor;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.Address.Remove
{
    public class RemoveResidentialAddressEditorConfiguration : EditorConfiguration<RemoveAddressEditor>,
                                                    IParentedEditorConfiguration<RemoveAddressActionConfiguration>
    {
        public RemoveResidentialAddressEditorConfiguration()
            : base("Remove Address", EditorAction.Delete)
        {
        }
    }
}