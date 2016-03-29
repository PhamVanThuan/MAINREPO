using SAHL.UI.Halo.Models.Account;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan;

namespace SAHL.UI.Halo.Configuration.Client.MortgageLoan
{
    public class LifeChildTileConfiguration : HaloSubTileConfiguration,
                                              IHaloChildTileConfiguration<MortgageLoanRootTileConfiguration>,
                                              IHaloTileModel<LifeChildModel>
    {
        public LifeChildTileConfiguration()
            : base("Life", "Life", 1, noOfRows: 2, noOfColumns: 2)
        {
        }
    }
}
