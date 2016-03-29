using SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.PostalAddress.Actions;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Editors;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Halo.Editors.LegalEntity.Address.UpdateAddressEditor;
using SAHL.Core.Data;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.Address.Update
{
    public class UpdateAddressEditorConfiguration : EditorConfiguration<UpdateAddressEditor>,
                                                         IParentedEditorConfiguration<UpdateAddressActionConfiguration>
    {
        public UpdateAddressEditorConfiguration()
            : base("Update Address", EditorAction.Update)
        {
        }
    }

    public class UpdateResidentialAddressEditorConfiguration : EditorConfiguration<UpdateAddressEditor>,
                                                        IParentedEditorConfiguration<SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.ResidentialAddress.Actions.UpdateAddressActionConfiguration>
    {
        public UpdateResidentialAddressEditorConfiguration()
            : base("Update Address", EditorAction.Update)
        {
        }
    }
}