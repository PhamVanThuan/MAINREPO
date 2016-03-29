using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;


namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_removing_empty_third_party_invoices : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int thirdPartyInvoiceKey;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            thirdPartyInvoiceKey = 123456;
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            testDataManager.RemoveEmptyThirdPartyInvoice(thirdPartyInvoiceKey);
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext()
                .WasToldTo(x=>x.Complete());
        };

        private It should_execute_the_correct_statement = () =>
        {
            fakeDb.FakedDb.InAppContext().
                WasToldTo(x => x.ExecuteNonQuery(Arg.Is<RemoveEmptyThirdPartyInvoiceStatement>(y => y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey)));
        };


    }
}
