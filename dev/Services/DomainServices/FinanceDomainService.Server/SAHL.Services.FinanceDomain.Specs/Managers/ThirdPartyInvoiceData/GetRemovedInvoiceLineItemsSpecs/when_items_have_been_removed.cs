using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData.GetRemovedInvoiceLineItemsSpecs
{
    public class when_items_have_been_removed : WithFakes
    {
        private static FakeDbFactory fakeDb;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static ThirdPartyInvoiceManager dataFilter;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static IEnumerable<InvoiceLineItemDataModel> existingInvoiceLineItems;
        private static IEnumerable<InvoiceLineItemDataModel> removedItems;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(fakeDb);
            dataFilter = new ThirdPartyInvoiceManager(dataManager);
            invoiceLineItems = new List<InvoiceLineItemModel>(){
                new InvoiceLineItemModel(1, 1, 1, 100.00M, true),
                new InvoiceLineItemModel(2, 1, 1, 100.00M, true)
            };
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
            removedItems = dataFilter.GetRemovedInvoiceLineItems(existingInvoiceLineItems, invoiceLineItems);
        };

        private It should_return_the_items_to_be_removed = () =>
        {
            removedItems.Where(x => x.InvoiceLineItemKey == 3 || x.InvoiceLineItemKey == 4).Count().ShouldEqual(2);
        };

        private It should_not_return_any_of_the_line_items_not_being_removed = () =>
        {
            removedItems.Where(x => x.InvoiceLineItemKey == invoiceLineItems.First().InvoiceLineItemKey
                || x.InvoiceLineItemKey == invoiceLineItems.Last().InvoiceLineItemKey)
                .FirstOrDefault()
                .ShouldBeNull();
        };
    }
}