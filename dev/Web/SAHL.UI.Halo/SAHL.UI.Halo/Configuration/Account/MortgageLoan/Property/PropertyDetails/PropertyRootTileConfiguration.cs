using SAHL.UI.Halo.Models.Account.Property;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.PropertyDetails
{
    public class PropertyRootTileConfiguration : HaloSubTileConfiguration,
                                                     IHaloRootTileConfiguration,
                                                     IHaloModuleTileConfiguration<ClientHomeConfiguration>,
                                                     IHaloTileModel<PropertyRootModel>
    {

        public PropertyRootTileConfiguration()
            : base("Property Details", "PropertyDetails", 3, noOfRows: 2)
        {

        }
    }
}
