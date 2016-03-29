using SAHL.Config.Services.Halo.Modules.Client.Editors.Address.ChangeDomiciliumAddress;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Halo.Editors.LegalEntity.Address.ChangeDomiciliumAddressEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.UI.Halo.Configuration.Modules.Client.Editors.Address.ChangeDomiciliumAddress.Pages.Selector
{
    public class ChangeDomiciliumAddressEditorPageSelectorConfiguration : IEditorPageSelectorConfiguration<ChangeDomiciliumAddressEditorConfiguration>
    {
        public Type PageSelectorType
        {
            get { return typeof(ChangeDomiciliumAddressEditorPageSelector); }
        }
    }
}
