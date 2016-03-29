using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Halo.Editors.LegalEntity.Address.UpdateAddressEditor;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.Address.Update.Pages.Selector
{
    public class UpdateAddressEditorPageSelectorConfiguration : IEditorPageSelectorConfiguration<UpdateAddressEditorConfiguration>
    {
        public System.Type PageSelectorType
        {
            get { return typeof(UpdateAddressEditorPageSelector); }
        }
    }
}