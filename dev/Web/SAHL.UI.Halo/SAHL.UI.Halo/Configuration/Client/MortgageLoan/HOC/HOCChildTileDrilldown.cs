using SAHL.UI.Halo.Configuration.Account.HOC;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Client.MortgageLoan.HOC
{
    public class HOCChildTileDrilldown : HaloTileActionDrilldownBase<HOCChildTileConfiguration, HOCRootTileConfiguration>,
                                                  IHaloTileActionDrilldown<HOCChildTileConfiguration>
    {
        public HOCChildTileDrilldown()
            : base("HOC")
        {
        }
    }
}