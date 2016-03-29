using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.LossControl.Invoices
{
    [HaloRole("Operations - Manager", "Invoice Processor")]
    public class InvoicesRootTileConfiguration : HaloSubTileConfiguration,
                                                 IHaloRootTileConfiguration,
                                                 IHaloModuleTileConfiguration<LossControlModuleConfiguration>
    {
        public InvoicesRootTileConfiguration()
            : base("Invoices", "Invoices", sequence: 1, startRow: 0, startColumn: 0, noOfRows: 12, noOfColumns: 12)
        {
        }
    }
}
