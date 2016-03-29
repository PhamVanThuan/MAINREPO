using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_removing_disability_claim_record : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int disabilityClaimKey;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            disabilityClaimKey = 111;
        };

        private Because of = () =>
        {
            testDataManager.RemoveDisabilityClaimRecord(disabilityClaimKey);
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
        
        private It should_execute_the_correct_statement_using_the_correct_disabilityClaimKey = () =>
        {
            fakeDb.FakedDb.InAppContext().
               WasToldTo
                (x => x.ExecuteNonQuery<int>
                  (Arg.Is<RemoveDisabilityClaimRecordStatement>(y => y.DisabilityClaimKey == disabilityClaimKey)));
        };
    }
}