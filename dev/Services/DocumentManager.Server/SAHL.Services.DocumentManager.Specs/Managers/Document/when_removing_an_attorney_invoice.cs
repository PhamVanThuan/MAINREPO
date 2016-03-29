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
    public class when_removing_an_attorney_invoice : WithFakes
    {
        private static DocumentDataManager documentManager;
        private static FakeDbFactory dbFactory;
        private static int attorneyInvoiceKey;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            documentManager = new DocumentDataManager(dbFactory);
            attorneyInvoiceKey = 123;
        };

        private Because of = () =>
        {
            documentManager.RemoveAttorneyInvoice(attorneyInvoiceKey);
        };

        private It should_insert_the_invoice_into_the_db = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.Delete(Arg.Any<ISqlStatement<int>>()));
        };

        private It should_call_complete = () =>
        {
            dbFactory.NewDb().InAppContext().WasToldTo(x => x.Complete());
        };
    }
}