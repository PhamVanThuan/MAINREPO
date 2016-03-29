using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData.GetRemovedInvoiceLineItemsSpecs
{
    public class when_items_have_not_been_removed : WithFakes
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
                new InvoiceLineItemDataModel(2,1,1,100.00M, true, 14.00M, 114.00M)
            };
        };

        private Because of = () =>
        {
            removedItems = dataFilter.GetRemovedInvoiceLineItems(existingInvoiceLineItems, invoiceLineItems);
       };

        private It should_return_an_empty_collection = () =>
        {
            removedItems.ShouldBeEmpty();
        };

   }
}