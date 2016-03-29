using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData.GetRemovedInvoiceLineItemsSpecs
{
    public class when_the_existing_invoice_has_no_line_items : WithFakes
    {
        private static FakeDbFactory fakeDb;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static ThirdPartyInvoiceManager dataFilter;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static IEnumerable<InvoiceLineItemDataModel> existingInvoiceLineItems;
        private static IEnumerable<InvoiceLineItemDataModel> removedLineItems;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(fakeDb);
            dataFilter = new ThirdPartyInvoiceManager(dataManager);
            existingInvoiceLineItems = Enumerable.Empty<InvoiceLineItemDataModel>();
            invoiceLineItems = new List<InvoiceLineItemModel>()
            {
                new InvoiceLineItemModel(1,1,1,100.00M, true),
                new InvoiceLineItemModel(2,1,1,100.00M, true),
                new InvoiceLineItemModel(3,1,1,100.00M, true),
                new InvoiceLineItemModel(4,1,1,100.00M, true)
            };
        };

        private Because of = () =>
        {
            removedLineItems = dataFilter.GetRemovedInvoiceLineItems(existingInvoiceLineItems, invoiceLineItems);
        };

        private It should_return_zero_line_items = () =>
        {
            removedLineItems.ShouldBeEmpty();
        };
    }
}