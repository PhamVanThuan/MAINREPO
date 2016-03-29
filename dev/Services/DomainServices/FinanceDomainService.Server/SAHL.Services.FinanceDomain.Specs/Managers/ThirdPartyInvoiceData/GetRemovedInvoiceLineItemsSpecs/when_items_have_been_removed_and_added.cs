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
    public class when_items_have_been_removed_and_added : WithFakes
    {
        private static FakeDbFactory fakeDb;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static ThirdPartyInvoiceManager dataFilter;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static IEnumerable<InvoiceLineItemDataModel> existingInvoiceLineItems;
        private static IEnumerable<InvoiceLineItemDataModel> removedItems;
        private static InvoiceLineItemModel newLineItem;

        private Establish context = () =>
        {
            newLineItem = new InvoiceLineItemModel(null, 1, 999, 999M, false);
            fakeDb = new FakeDbFactory();
            dataManager = new ThirdPartyInvoiceDataManager(fakeDb);
            dataFilter = new ThirdPartyInvoiceManager(dataManager);
            invoiceLineItems = new List<InvoiceLineItemModel>(){
                new InvoiceLineItemModel(1, 1, 1, 100.00M, true),
                new InvoiceLineItemModel(2, 1, 1, 100.00M, true),
                newLineItem
            };
            existingInvoiceLineItems = new List<InvoiceLineItemDataModel>()
            {
                new InvoiceLineItemDataModel(1,1,1,100.00M, true, 14.00M, 114.00M),
                new InvoiceLineItemDataModel(2,1,1,100.00M, true, 14.00M, 114.00M),
                new InvoiceLineItemDataModel(3,1,1,100.00M, true, 14.00M, 114.00M)
            };
        };

        private Because of = () =>
        {
            removedItems = dataFilter.GetRemovedInvoiceLineItems(existingInvoiceLineItems, invoiceLineItems);
        };

        private It should_correctly_filter_through_expected_number_of_removed_items = () =>
        {
            removedItems.Count().ShouldEqual(1);
        };

        private It should_filter_through_the_actual_removed_items = () =>
        {
            removedItems.Where(x => x.InvoiceLineItemKey == 3).Count().ShouldEqual(1);
        };

        private It should_not_filter_through_any_of_the_line_items_not_being_removed = () =>
        {
            removedItems.Where(x => x.InvoiceLineItemKey == invoiceLineItems.First().InvoiceLineItemKey
                || x.InvoiceLineItemKey == invoiceLineItems.Last().InvoiceLineItemKey)
                .FirstOrDefault()
                .ShouldBeNull();
        };

        private It should_filter_out_the_new_line_item = () =>
        {
            removedItems.ShouldNotContain(newLineItem);
        };
    }
}