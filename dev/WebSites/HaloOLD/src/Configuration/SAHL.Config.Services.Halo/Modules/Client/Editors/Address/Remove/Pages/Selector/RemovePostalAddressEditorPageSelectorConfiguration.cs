﻿using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Halo.Editors.Address.RemoveAddressEditor;
using System;

namespace SAHL.Config.Services.Halo.Modules.Client.Editors.Address.Remove.Pages.Selector
{
    public class RemovePostalAddressEditorPageSelectorConfiguration : IEditorPageSelectorConfiguration<RemovePostalAddressEditorConfiguration>
    {
        public Type PageSelectorType
        {
            get { return typeof(RemoveAddressEditorPageSelector); }
        }
    }
}