using SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.Address.Actions;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Editors;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Halo.Editors.Address.ChangeDomiciliumAddressEditor;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.Address.ChangeDomiciliumAddress
{
    public class ChangeDomiciliumAddressEditorConfiguration : EditorConfiguration<ChangeDomiciliumAddressEditor>,
                                                         IParentedEditorConfiguration<ChangeDomiciliumAddressActionConfiguration>
    {
        public ChangeDomiciliumAddressEditorConfiguration()
            : base("Change Domicilium Address", EditorAction.Update)
        {
        }
    }
}