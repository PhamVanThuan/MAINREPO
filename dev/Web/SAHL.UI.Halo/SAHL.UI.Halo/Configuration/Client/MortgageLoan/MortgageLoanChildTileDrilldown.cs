using SAHL.UI.Halo.Configuration.Account.MortgageLoan;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.MortgageLoan
{
    public class MortgageLoanChildTileDrilldown : HaloTileActionDrilldownBase<MortgageLoanChildTileConfiguration, MortgageLoanRootTileConfiguration>,
                                                  IHaloTileActionDrilldown<MortgageLoanChildTileConfiguration>
    {
        public MortgageLoanChildTileDrilldown()
            : base("Mortgage Loan")
        {
        }
    }
}