using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs

{
    public class when_removing_active_new_business_applicants_records : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int offerRoleKey;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            offerRoleKey = 5285869;
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            testDataManager.RemoveActiveNewBusinessApplicantRecord(offerRoleKey);
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

        private It should_execute_the_statement_using_the_correct_offerRoleKey = () =>
        {
            fakeDb.FakedDb.InAppContext().
                WasToldTo
                  (x => x.ExecuteNonQuery<int>
                      (Arg.Is<RemoveActiveNewBusinessApplicantStatement>(y => y.OfferRoleKey == offerRoleKey)));
        };


    }
}