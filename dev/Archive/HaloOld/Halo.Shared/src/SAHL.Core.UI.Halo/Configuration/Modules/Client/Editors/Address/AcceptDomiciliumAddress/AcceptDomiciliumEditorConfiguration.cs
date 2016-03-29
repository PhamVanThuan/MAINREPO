using SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.Address.Actions;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Editors;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Halo.Editors.Address.AcceptDomiciliumEditor;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.Address.AcceptDomiciliumAddress
{
    public class AcceptDomiciliumEditorConfiguration : EditorConfiguration<AcceptDomiciliumEditor>,
                                                       IParentedEditorConfiguration<AcceptDomiciliumActionConfiguration>
    {
        public AcceptDomiciliumEditorConfiguration()
            : base("Accept Domicilium", EditorAction.Update)
        {
        }
    }
}