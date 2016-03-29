using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_inserting_a_disability_claim_record : WithFakes
    {
        private static TestDataManager testDataManager;
        private static DisabilityClaimDataModel disabilityClaimDataModel;
        private static FakeDbFactory fakeDb;
        private static int AccountKey;

        private Establish context = () =>
        {
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
            AccountKey = 1235888;
            disabilityClaimDataModel = new DisabilityClaimDataModel(AccountKey, Param.IsAny<int>(), Param.IsAny<DateTime>(), Param.IsAny<DateTime>(),
                                                                    Param.IsAny<DateTime>(), Param.IsAny<string>(), Param.IsAny<int>(), Param.IsAny<string>(),
                                                                    Param.IsAny<DateTime>(), Param.IsAny<int>(), Param.IsAny<DateTime>(), Param.IsAny<int>(),
                                                                    Param.IsAny<DateTime>());
        };

        private Because of = () =>
        {
            testDataManager.InsertDisabilityClaimRecord(disabilityClaimDataModel);
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

        private It should_insert_record_for_the_correct_account_key = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo
                (x => x.Insert<DisabilityClaimDataModel>((disabilityClaimDataModel)));
        };
    }
}