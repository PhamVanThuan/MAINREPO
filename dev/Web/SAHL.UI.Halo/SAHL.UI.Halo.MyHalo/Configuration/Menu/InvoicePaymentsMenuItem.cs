using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.Menu
{
    [HaloRole("Operations - Manager", "Invoice Payment Processor")]
    public class InvoicePaymentsMenuItem : HaloMenuItem, IHaloApplicationMenuItem<MyHaloHaloApplicationConfiguration>
    {
        public InvoicePaymentsMenuItem()
            : base("Invoice Payments", "Invoice Payments", 3)
        {

        }
    }
}
