using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.LossControlManager
{
    [HaloRole("Operations - Manager", "Invoice Payment Processor")]
    public class InvoicePaymentsRootTileConfiguration : HaloSubTileConfiguration,
                                           IHaloRootTileConfiguration,
                                           IHaloModuleTileConfiguration<InvoicePaymentsModuleConfiguration>
    {
        public InvoicePaymentsRootTileConfiguration()
            : base("Invoice Payments Portal Page", "Invoice Payments", sequence: 1, startRow: 0, startColumn: 0, noOfRows: 100, noOfColumns: 12)
        {
        }
    }
}