using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Managers.ThirdPartyInvoiceData.GetInvoiceLineItemsSpecs
{
    public class when_getting_an_invoices_line_items : WithFakes
    {
        private static FakeDbFactory fakeDb;
        private static ThirdPartyInvoiceDataManager dataManager;
        private static IEnumerable<InvoiceLineItemDataModel> invoiceLineItems;
        private static int thirdPartyInvoiceKey;
        private static string expectedWhereClause;
        private static IEnumerable<InvoiceLineItemDataModel> lineItems;

        private Establish context = () =>
            {
                lineItems = new List<InvoiceLineItemDataModel>()
                {
                    new InvoiceLineItemDataModel(1, 1, 1, 100.00M, true, 14.00M, 114.00M)
                };
                thirdPartyInvoiceKey = 384957;
                expectedWhereClause = string.Format("[ThirdPartyInvoiceKey] = {0}", thirdPartyInvoiceKey);
                fakeDb = new FakeDbFactory();
                dataManager = new ThirdPartyInvoiceDataManager(fakeDb);
                fakeDb.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectWhere<InvoiceLineItemDataModel>(expectedWhereClause, null))
                    .Return(lineItems);
            };

        private Because of = () =>
            {
                invoiceLineItems = dataManager.GetInvoiceLineItems(thirdPartyInvoiceKey);
            };

        private It should_query_using_the_key_provided = () =>
            {
                fakeDb.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectWhere<InvoiceLineItemDataModel>(expectedWhereClause, null));
            };

        private It should_return_the_line_items_from_the_database = () =>
            {
                invoiceLineItems.Count().ShouldEqual(lineItems.Count());
                invoiceLineItems.ShouldContain(lineItems.First());
            };
    }
}