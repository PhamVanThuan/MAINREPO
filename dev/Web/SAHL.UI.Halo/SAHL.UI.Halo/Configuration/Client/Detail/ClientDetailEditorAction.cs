using SAHL.UI.Halo.Configuration.Client.Detail;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Editors
{
    public class ClientDetailEditorAction : HaloTileActionEditBase<ClientDetailRootTileConfiguration>,
                                              IHaloTileActionEdit<ClientDetailRootTileConfiguration>
    {
        public ClientDetailEditorAction()
            : base("Edit Client", iconName: "fa fa-pencil fontawesome", sequence: 1)
        {
        }
    }
}