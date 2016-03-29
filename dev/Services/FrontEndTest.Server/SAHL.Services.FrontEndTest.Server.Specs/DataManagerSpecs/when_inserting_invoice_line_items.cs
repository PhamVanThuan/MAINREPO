using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_inserting_invoice_line_items : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static IEnumerable<InvoiceLineItemDataModel> invoiceLineItems;

        Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            invoiceLineItems = new List<InvoiceLineItemDataModel>() { 
                new InvoiceLineItemDataModel(1, 0, 0.0M, true, null, 0.0M), 
                new InvoiceLineItemDataModel(2, 0, 0.0M, true, null, 0.0M) };
            
        };

        Because of = () =>
        {
            testDataManager.InsertInvoiceLineItems(invoiceLineItems);
        };

        It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

        It should_insert_invoice_line_items = () =>
        {
            fakeDb.FakedDb.InAppContext()
                .WasToldTo(x => x.Insert<InvoiceLineItemDataModel>(
                    Arg.Is<IEnumerable<InvoiceLineItemDataModel>>(y => y.Count() == 2)));
        };
		
    }
}
