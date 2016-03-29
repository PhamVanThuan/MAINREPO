using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Configuration.Editors;
using SAHL.Core.UI.Halo.Editors.Address.ChangeDomiciliumAddressEditor.Pages;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.Address.ChangeDomiciliumAddress.Pages
{
    public class ChangeDomiciliumAddressEditorPage1Configuration : EditorOrderedPageConfiguration<ChangeDomiciliumAddressEditorPage1Model>,
                                                              IParentedEditorPageConfiguration<ChangeDomiciliumAddressEditorConfiguration>
    {
        public ChangeDomiciliumAddressEditorPage1Configuration()
            : base(1)
        {
        }
    }
}