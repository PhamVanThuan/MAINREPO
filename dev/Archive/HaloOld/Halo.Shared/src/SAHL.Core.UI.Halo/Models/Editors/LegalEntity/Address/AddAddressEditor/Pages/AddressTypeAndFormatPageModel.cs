using System.Collections.Generic;
using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Halo.Common;
using SAHL.Core.UI.Halo.Editors.Address.Common.Pages;
using SAHL.Core.UI.Models;

namespace SAHL.Core.UI.Halo.Editors.Address.AddAddressEditor.Pages
{
    public class AddressTypeAndFormatPageModel : AbstractAddressTypeAndFormatPageModelBase, IEditorPageModel
    {
        public void Initialise(BusinessContext businessContext)
        {
            this.AddressTypes = new NamedKey[] { new NamedKey("Residential", 1), new NamedKey("Postal", 2) };
            this.SelectedAddressTypeKey = 1;

            this.AddressFormatTypes = new NamedKey[] { new NamedKey("Street", 1), new NamedKey("Box", 2), new NamedKey("PostNetSuite",3),
                                                     new NamedKey("Private Bag", 4), new NamedKey("Free Text",5), new NamedKey("Cluster Box", 6) };
            this.SelectedAddressFormatKey = 1;
        }

        public IEnumerable<NamedKey> AddressTypes { get; set; }

        public IEnumerable<NamedKey> AddressFormatTypes { get; set; }

        public int SelectedAddressTypeKey { get; set; }

        public int SelectedAddressFormatKey { get; set; }
    }
}