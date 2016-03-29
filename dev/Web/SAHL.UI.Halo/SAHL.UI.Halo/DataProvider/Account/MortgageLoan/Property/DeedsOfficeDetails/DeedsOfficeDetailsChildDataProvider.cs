using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Property.DeedsOfficeDetails;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.Account.MortgageLoan.Property.DeedsOfficeDetails
{
    public class DeedsOfficeDetailsChildDataProvider : HaloTileBaseChildDataProvider,
                                                        IHaloTileChildDataProvider<DeedsOfficeDetailsChildTileConfiguration>
    {
        public DeedsOfficeDetailsChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }
    }
}