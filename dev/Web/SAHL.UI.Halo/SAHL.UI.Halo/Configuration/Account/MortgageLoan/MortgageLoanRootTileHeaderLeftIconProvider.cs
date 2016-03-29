using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan
{
    public class MortgageLoanRootTileHeaderLeftIconProvider : HaloTileHeaderIconProviderBase<MortgageLoanRootTileHeaderConfiguration>
    {
        public MortgageLoanRootTileHeaderLeftIconProvider()
            : base(HaloTileIconAlignment.Left)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            // No action required
        }
    }
}