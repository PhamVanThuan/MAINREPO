using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_removing_a_valuer
    {
        private static TestDataManager testDataManager;
        private static ValuatorDataModel valuator;
        private static byte limitedUserGroupKey;
        private static string valuatorContact;
        private static int legalEntityKey;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
         {
             fakeDb = new FakeDbFactory();
             testDataManager = new TestDataManager(fakeDb);
             valuatorContact = "VishavP";
             legalEntityKey = 1235888;
             limitedUserGroupKey = 1;
             valuator = new ValuatorDataModel(valuatorContact, "!@#$%^&*()", limitedUserGroupKey, (int)GeneralStatus.Active, legalEntityKey);
         };

        private Because of = () =>
         {
             testDataManager.RemoveValuer(valuator.ValuatorKey);
         };

        private It should_delete_the_valuater = () =>
         {
             fakeDb.FakedDb.InAppContext().
                WasToldTo(x => x.DeleteByKey<ValuatorDataModel, int>(Param<int>.Matches(y=> y == valuator.ValuatorKey)));
         };

        private It should_complete_the_db_context = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
         };

    }
}
