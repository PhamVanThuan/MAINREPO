using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.MyHalo.Configuration.LossControlManager
{
    public class InvoicePaymentsModuleConfiguration : HaloModuleConfiguration,
                                                  IHaloModuleApplicationConfiguration<MyHaloHaloApplicationConfiguration>
    {
        public InvoicePaymentsModuleConfiguration()
            : base("Invoice Payments", 1)
        {

        }
    }
}
