using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.DocumentManager.Managers.Document;
using SAHL.Services.Interfaces.DocumentManager.Models;
using System;
using System.Linq;

namespace SAHL.Services.DocumentManager.Specs.Managers.Document
{
    public class when_saving_an_attorney_invoice : WithFakes
    {
        private static DocumentDataManager documentManager;
        private static ThirdPartyInvoiceDocumentModel invoice;
        private static string originalFileName;
        private static string dataStoreGuid;
        private static int storeID;
        private static FakeDbFactory dbFactory;
        private static DateTime dateArchived;
        private static int thirdPartyInvoiceKey;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            documentManager = new DocumentDataManager(dbFactory);
            storeID = 44;
            originalFileName = "INV3-Some Invoice";
            dateArchived = DateTime.Now;
            thirdPartyInvoiceKey = 104239;
            dataStoreGuid = Guid.NewGuid().ToString();
            invoice = new ThirdPartyInvoiceDocumentModel("12345", DateTime.Now, DateTime.Now, "test@some.email.com", "12345 - Some Invoice", "INV3-Some Invoice", "pdf", "Category", "Some Contetnt");
        };

        private Because of = () =>
        {
            documentManager.SaveAttorneyInvoiceFile(invoice, thirdPartyInvoiceKey.ToString(), dataStoreGuid, originalFileName, storeID, dateArchived);
        };

        private It should_insert_the_invoice_into_the_db = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.Insert(Arg.Any<ISqlStatement<ThirdPartyInvoiceDocumentModel>>()));
        };

        private It should_call_complete = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.Complete());
        };
    }
}