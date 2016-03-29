using SAHL.UI.Halo.Configuration.Account.MortgageLoan;
using SAHL.UI.Halo.Models.Client.HOC;
using SAHL.UI.Halo.Models.Client.MortgageLoan;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.MortgageLoan.HOC
{
    public class HOCChildTileConfiguration : HaloSubTileConfiguration,
                                                      IHaloChildTileConfiguration<MortgageLoanRootTileConfiguration>,
                                                      IHaloTileModel<HOCChildModel>
    {
        public HOCChildTileConfiguration()
            : base("HOC", "HOC", 1, noOfRows: 2, noOfColumns: 2)
        {
        }
    }
}