using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_removing_open_business_application_command : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int applicationNumber;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            applicationNumber = 1711511;
        };

        private Because of = () =>
        {
            testDataManager.RemoveOpenNewBusinessApplicationCommand(applicationNumber);
        };

        private It should_remove_the_correct_application = () =>
        {
            fakeDb.FakedDb.InAppContext().
              WasToldTo
               (x => x.ExecuteNonQuery<int>
                   (Arg.Is<RemoveOpenNewBusinessApplicationStatement>(y => y.ApplicationNumber == applicationNumber)));
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

    }
}