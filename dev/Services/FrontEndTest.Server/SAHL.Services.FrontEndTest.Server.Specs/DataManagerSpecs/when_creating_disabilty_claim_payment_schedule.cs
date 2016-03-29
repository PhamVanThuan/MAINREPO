using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Services.FrontEndTest.Managers.Statements;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_creating_disabilty_claim_payment_schedule : WithFakes
    {
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int disabilityClaimKey;
        private static string adUserName;
        private static string expectedAnswer;

        private Establish context = () =>
        {
            disabilityClaimKey = 111;
            adUserName = "VishavP";
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            fakeDb.FakedDb.InReadOnlyAppContext().
                WhenToldTo
                  (x => x.SelectOne<string>(Param.IsAny<CreateDisabilityClaimPaymentScheduleStatement>()))
                    .Return("Griever");
        };

        private Because of = () =>
        {
            expectedAnswer = testDataManager.CreateDisabilityClaimPaymentSchedule(disabilityClaimKey, adUserName);
        };
        
        private It should_use_a_string_for_creating_the_disability_payment_schedule = () =>
        {
            fakeDb.FakedDb.InReadOnlyAppContext().
                WasToldTo
                  (x => x.SelectOne<string>(Arg.Is<CreateDisabilityClaimPaymentScheduleStatement>(y=>y.DisabilityClaimKey == disabilityClaimKey
                      && y.UserName == adUserName)));
        };

        private It should_return_the_correct_answer = () =>
        {
            expectedAnswer.ShouldBeTheSameAs("Griever");
        };
    }
}