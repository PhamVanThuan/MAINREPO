using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.HOC
{
    public class HOCRootTileHeaderLeftIconProvider : HaloTileHeaderIconProviderBase<HOCRootTileHeaderConfiguration>
    {
        public HOCRootTileHeaderLeftIconProvider()
            : base(HaloTileIconAlignment.Left)
        {
        }

        public override void Execute<TDataModel>(TDataModel dataModel)
        {
            // No action required
        }
    }
}