using SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.Actions;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Editors;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Halo.Editors.Address.AddAddressEditor;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.Address.Add
{
    public class AddAddressEditorConfiguration : EditorConfiguration<AddAddressEditor>,
                                                         IParentedEditorConfiguration<AddAddressActionConfiguration>
    {
        public AddAddressEditorConfiguration()
            : base("Add Address", EditorAction.Add)
        {
        }
    }
}