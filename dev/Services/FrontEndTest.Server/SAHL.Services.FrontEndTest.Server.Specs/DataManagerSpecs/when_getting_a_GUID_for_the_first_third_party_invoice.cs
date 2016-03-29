using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_getting_a_GUID_for_the_first_third_party_invoice : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static GetFirstThirdPartyInvoiceWithDocumentStatement statement;
        private static string expectedResult;
        private static string actualResult;

        private Establish context = () =>
         {
             fakeDb = new FakeDbFactory();
             testDataManager = new TestDataManager(fakeDb);
             statement = new GetFirstThirdPartyInvoiceWithDocumentStatement();
             actualResult = testDataManager.GetGuidForFirstThirdPartyInvoice();
         };

        private Because of = () =>
         {
             expectedResult = testDataManager.GetGuidForFirstThirdPartyInvoice();
         };

        private It should_return_the_correct_statement = () =>
         {
             actualResult.ShouldEqual(expectedResult);
         };
    }
}