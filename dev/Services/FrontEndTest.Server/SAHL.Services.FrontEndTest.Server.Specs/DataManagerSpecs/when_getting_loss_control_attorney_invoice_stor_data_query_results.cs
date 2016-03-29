using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.QueryStatements;
using SAHL.Services.Interfaces.FrontEndTest.Models;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_getting_loss_control_attorney_invoice_stor_data_query_results : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int thirdPartyInvoicekey;
        private static GetLossControlAttorneyInvoiceStorDataQueryResult expectedAnswer;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            thirdPartyInvoicekey = 2580;
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            expectedAnswer = testDataManager.GetLossControlAttorneyInvoiceStorData(thirdPartyInvoicekey);
        };

        private It should_get_the_loss_control_invoice_stor_data = () =>
        {
            fakeDb.FakedDb.InReadOnlyAppContext()
                .WasToldTo
                    (x => x.SelectOne<GetLossControlAttorneyInvoiceStorDataQueryResult>
                        (Arg.Is<GetLossControlAttorneyInvoiceStorDataQueryStatement>(y=>y.ThirdPartyInvoiceKey == thirdPartyInvoicekey)));
        };

        private It should_return_the_correct_answer = () =>
        {
            expectedAnswer.ShouldEqual(testDataManager.GetLossControlAttorneyInvoiceStorData(thirdPartyInvoicekey));
        };
    }
}
