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
    public class when_given_an_empty_list_of_new_line_items : WithFakes
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
            invoiceLineItems = Enumerable.Empty<InvoiceLineItemModel>();
            existingInvoiceLineItems = new List<InvoiceLineItemDataModel>()
            {
                new InvoiceLineItemDataModel(1,1,1,100.00M, true, 14.00M, 114.00M),
                new InvoiceLineItemDataModel(2,1,1,100.00M, true, 14.00M, 114.00M),
                new InvoiceLineItemDataModel(3,1,1,100.00M, true, 14.00M, 114.00M),
                new InvoiceLineItemDataModel(4,1,1,100.00M, true, 14.00M, 114.00M)
            };
        };

        private Because of = () =>
        {
            removedLineItems = dataFilter.GetRemovedInvoiceLineItems(existingInvoiceLineItems, invoiceLineItems);
        };

        private It should_return_all_lines_from_the_existing_invoice = () =>
        {
            removedLineItems.Count().ShouldEqual(existingInvoiceLineItems.Count());
        };

    }
}