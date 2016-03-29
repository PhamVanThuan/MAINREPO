using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.Life
{
    public class LifeRootTileHeaderLeftIconProvider : HaloTileHeaderIconProviderBase<LifeRootTileHeaderConfiguration>
    {
        public LifeRootTileHeaderLeftIconProvider()
            : base(HaloTileIconAlignment.Left)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            // No action required
        }
    }
}