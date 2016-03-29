using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_removing_disability_payment_records : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int DisabilityClaimKey;

        private Establish context = () =>
        {
            DisabilityClaimKey = 908;
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            testDataManager.RemoveDisabilityPaymentRecord(DisabilityClaimKey);
        };

        private It should_remove_the_correct_disability_payment_record = () =>
        {
            fakeDb.FakedDb.InAppContext().
                WasToldTo
                  (x => x.ExecuteNonQuery<int>
                      (Arg.Is<RemoveDisabilityPaymentRecordStatement>(y => y.DisabilityClaimKey == DisabilityClaimKey)));
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}