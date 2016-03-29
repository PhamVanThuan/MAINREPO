using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Home.LossControlManager
{
    public class MonthBreakdownByAttorneyRootTileConfiguration : HaloSubTileConfiguration,
                                           IHaloRootTileConfiguration,
                                           IHaloModuleTileConfiguration<HomeModuleConfiguration>
    {
        public MonthBreakdownByAttorneyRootTileConfiguration()
            : base("This Month Breakdown By Attorney", "Monthly Breakdown By Attorney", sequence: 1, startRow: 0, startColumn: 0, noOfRows: 12, noOfColumns: 9)
        {
        }
    }
}