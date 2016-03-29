using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData
{
    public class when_removing_invoice_line_item : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager dataManager;
        private static FakeDbFactory dbFactory;
        private static int removedInvoiceLineItemKey;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(dbFactory);
            removedInvoiceLineItemKey = 1001;
        };

        private Because of = () =>
        {
            dataManager.RemoveInvoiceLineItem(removedInvoiceLineItemKey);
        };

        private It should_add_invoice_line_item = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.DeleteByKey<InvoiceLineItemDataModel, int>(removedInvoiceLineItemKey));
        };

        private It should_complete_the_context = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}