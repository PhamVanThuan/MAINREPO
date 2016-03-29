using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData.GetUpdatedInvoicedLineItemsSpecs
{
    public class when_line_items_have_been_updated : WithFakes
    {
        private static FakeDbFactory fakeDb;
        private static IThirdPartyInvoiceDataManager dataManager;
        private static ThirdPartyInvoiceManager dataFilter;
        private static InvoiceLineItemModel lineItem1;
        private static InvoiceLineItemModel lineItem2;
        private static InvoiceLineItemModel newLineItem;
        private static InvoiceLineItemDataModel existingLineItem1FromDb;
        private static InvoiceLineItemDataModel existingLineItem2FromDb;
        private static IEnumerable<InvoiceLineItemModel> lineItemsOnAmendedInvoice;
        private static IEnumerable<InvoiceLineItemModel> updatedLineItems;

        private Establish context = () =>
        {
            newLineItem = new InvoiceLineItemModel(null, 111, 2, 6000.00M, true);
            lineItem1 = new InvoiceLineItemModel(123, 111, 1, 100.00M, true);
            existingLineItem1FromDb = new InvoiceLineItemDataModel(123, 111, 1, 200M, true, 28.00M, 128.00M);
            lineItem2 = new InvoiceLineItemModel(124, 111, 1, 100.00M, false);
            existingLineItem2FromDb = new InvoiceLineItemDataModel(124, 111, 1, 200M, false, 0.00M, 200.00M);
            lineItemsOnAmendedInvoice = new List<InvoiceLineItemModel>() { lineItem1, lineItem2, newLineItem };
            fakeDb = new FakeDbFactory();
            fakeDb.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOneWhere<InvoiceLineItemDataModel>("[InvoiceLineItemKey] = 123", null))
                .Return(existingLineItem1FromDb);
            fakeDb.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOneWhere<InvoiceLineItemDataModel>("[InvoiceLineItemKey] = 124", null))
                .Return(existingLineItem2FromDb);
            dataManager = new ThirdPartyInvoiceDataManager(fakeDb);
            dataFilter = new ThirdPartyInvoiceManager(dataManager);
        };

        private Because of = () =>
        {
            updatedLineItems = dataFilter.GetUpdatedInvoicedLineItems(lineItemsOnAmendedInvoice);
        };

        private It should_return_updated_line_item1 = () =>
        {
            updatedLineItems.ShouldContain(lineItem1);
        };

        private It should_return_updated_line_item2 = () =>
        {
            updatedLineItems.ShouldContain(lineItem2);
        };

        private It should_fetch_existing_line_1 = () =>
        {
            fakeDb.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOneWhere<InvoiceLineItemDataModel>("[InvoiceLineItemKey] = 124", null));
        };

        private It should_fetch_existing_line_2 = () =>
        {
            fakeDb.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOneWhere<InvoiceLineItemDataModel>("[InvoiceLineItemKey] = 123", null));
        };

        private It should_not_contain_the_new_line_item = () =>
        {
            updatedLineItems.ShouldNotContain(newLineItem);
        };
    }
}