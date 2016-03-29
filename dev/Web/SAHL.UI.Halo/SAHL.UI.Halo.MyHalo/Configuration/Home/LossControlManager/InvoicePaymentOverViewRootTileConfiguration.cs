using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Home.LossControlManager
{
    public class InvoicePaymentOverViewRootTileConfiguration : HaloSubTileConfiguration,
                                           IHaloRootTileConfiguration,
                                           IHaloModuleTileConfiguration<HomeModuleConfiguration>
    {
        public InvoicePaymentOverViewRootTileConfiguration()
            : base("Invoice Payment Overview", "Invoice Payment Overview", sequence: 3, startRow: 0, startColumn: 9, noOfRows: 12, noOfColumns: 3)
        {
        }
    }
}