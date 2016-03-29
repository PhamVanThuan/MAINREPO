using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_removing_application_mailing_address : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int applicationNumber;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            applicationNumber = 1711500;
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            testDataManager.RemoveApplicationMailingAddress(applicationNumber);
        };

        private It should_remove_the_correct_application_using_the_correct_statement = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo
              (x => x.Delete<OfferMailingAddressDataModel>
                 (Arg.Is<RemoveApplicationMailingAddressStatement>(y => y.ApplicationNumber == applicationNumber)));
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}