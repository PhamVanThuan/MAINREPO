using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.MortgageLoan
{
    public class MortgageLoanChildTileHeaderLeftIconProvider : HaloTileHeaderIconProviderBase<MortgageLoanChildTileHeaderConfiguration>
    {
        public MortgageLoanChildTileHeaderLeftIconProvider()
            : base(HaloTileIconAlignment.Left)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            // No action required
        }
    }
}