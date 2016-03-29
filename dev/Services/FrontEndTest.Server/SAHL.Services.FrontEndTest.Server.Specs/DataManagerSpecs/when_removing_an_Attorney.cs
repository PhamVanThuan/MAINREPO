using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_removing_an_attorney
    {
        private static int attorneyKey;
        private static int legalEntityKey;
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
          {
              fakeDb = new FakeDbFactory();
              testDataManager = new TestDataManager(fakeDb);
              legalEntityKey = 1235888;
              attorneyKey = 123456789;
          };

        private Because of = () =>
         {
             testDataManager.RemoveAttorney(attorneyKey);
         };

        private It should_remove_the_specified_attorney = () =>
         {
             fakeDb.FakedDb.InAppContext().
                WasToldTo(x => x.DeleteByKey<AttorneyDataModel, int>(Param<int>.Matches(y => y == attorneyKey)));
         };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

    }
}
