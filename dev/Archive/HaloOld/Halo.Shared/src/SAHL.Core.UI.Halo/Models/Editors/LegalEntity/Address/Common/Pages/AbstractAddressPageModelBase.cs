using SAHL.Core.UI.Halo.Common;
using System;
using System.Collections.Generic;

namespace SAHL.Core.UI.Halo.Editors.Address.Common.Pages
{
    public abstract class AbstractAddressPageModelBase
    {
        public AbstractAddressPageModelBase()
        {
            this.SelectedAddressStatusKey = 1;
            this.AddressStatuses = new NamedKey[] { new NamedKey("Active", 1), new NamedKey("Inactive", 2) };
        }
        public DateTime EffectiveDate { get; set; }

        public IEnumerable<NamedKey> AddressStatuses { get; protected set; }

        public int SelectedAddressStatusKey { get; set; }
    }
}