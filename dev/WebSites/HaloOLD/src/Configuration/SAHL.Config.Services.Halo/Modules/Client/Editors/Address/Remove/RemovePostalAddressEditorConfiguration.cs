using SAHL.Config.Services.Halo.Modules.Client.Tiles.ClientDashboard.ClientDetail.PostalAddress.Actions;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Editors;
using SAHL.Core.UI.Enums;
using SAHL.Core.UI.Halo.Editors.Address.RemoveAddressEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.Address.Remove
{
    public class RemovePostalAddressEditorConfiguration : EditorConfiguration<RemoveAddressEditor>,
                                                          IParentedEditorConfiguration<RemoveAddressActionConfiguration>
    {
        public RemovePostalAddressEditorConfiguration()
            :base("Remove Address", EditorAction.Delete)
        {

        }
    }
}
