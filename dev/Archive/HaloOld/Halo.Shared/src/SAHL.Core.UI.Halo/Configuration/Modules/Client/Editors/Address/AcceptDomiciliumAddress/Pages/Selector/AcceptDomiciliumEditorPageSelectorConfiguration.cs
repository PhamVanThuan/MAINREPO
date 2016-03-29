using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Halo.Editors.Address.AcceptDomiciliumEditor;
using System;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.Address.AcceptDomiciliumAddress.Pages.Selector
{
    public class AcceptDomiciliumEditorPageSelectorConfiguration : IEditorPageSelectorConfiguration<AcceptDomiciliumEditorConfiguration>
    {
        public Type PageSelectorType
        {
            get { return typeof(AcceptDomiciliumEditorPageSelector); }
        }
    }
}