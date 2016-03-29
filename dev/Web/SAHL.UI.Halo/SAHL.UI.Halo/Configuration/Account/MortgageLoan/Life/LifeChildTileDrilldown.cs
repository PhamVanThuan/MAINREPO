using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Account.Life;
using SAHL.UI.Halo.Configuration.Client.MortgageLoan;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Life
{
    public class LifeChildTileDrilldown : HaloTileActionDrilldownBase<LifeChildTileConfiguration, LifeRootTileConfiguration>,
                                          IHaloTileActionDrilldown<LifeChildTileConfiguration>
    {
        public LifeChildTileDrilldown() : base("Life")
        {
        }
    }
}
