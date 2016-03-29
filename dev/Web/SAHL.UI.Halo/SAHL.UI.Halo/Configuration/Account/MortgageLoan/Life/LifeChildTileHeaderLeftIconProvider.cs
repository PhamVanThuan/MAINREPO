using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Life
{
    public class LifeChildTileHeaderLeftIconProvider : HaloTileHeaderIconProviderBase<LifeChildTileHeaderConfiguration>
    {
        public LifeChildTileHeaderLeftIconProvider() : base(HaloTileIconAlignment.Left)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            // No action required
        }
    }
}
