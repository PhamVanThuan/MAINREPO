using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Home.LossControlManager
{
    public class PaidInvoicesPerDayRootTileConfiguration : HaloSubTileConfiguration,
                                           IHaloRootTileConfiguration,
                                           IHaloModuleTileConfiguration<HomeModuleConfiguration>
    {
        public PaidInvoicesPerDayRootTileConfiguration()
            : base("Accumulative Number Invoices Paid - Previous vs This Month", "Paid Invoices Per Day", sequence: 2, startRow: 6, startColumn: 0, noOfRows: 6, noOfColumns: 8)
        {
        }
    }
}