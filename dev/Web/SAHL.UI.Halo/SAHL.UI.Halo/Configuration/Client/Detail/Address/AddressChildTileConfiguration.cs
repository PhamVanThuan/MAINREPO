using SAHL.UI.Halo.Models.Common.LegalEntityAddress;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.Detail.Address
{
    public class AddressChildTileConfiguration : HaloSubTileConfiguration,
                                                            IHaloChildTileConfiguration<ClientDetailRootTileConfiguration>,
                                                            IHaloTileModel<LegalEntityAddressChildModel>
    {
        public AddressChildTileConfiguration()
            : base("Address", "ClientAddress", 1, noOfRows: 2, noOfColumns: 2)
        {
        }
    }
}