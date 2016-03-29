using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Halo.Editors.Address.AddAddressEditor;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.Address.Add.Pages.Selector
{
    public class AddAddressEditorPageSelectorConfiguration : IEditorPageSelectorConfiguration<AddAddressEditorConfiguration>
    {
        public System.Type PageSelectorType
        {
            get { return typeof(AddAddressEditorPageSelector); }
        }
    }
}