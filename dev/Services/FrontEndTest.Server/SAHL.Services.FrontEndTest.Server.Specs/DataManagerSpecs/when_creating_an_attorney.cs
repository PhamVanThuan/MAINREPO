using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using NSubstitute;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_creating_an_attorney
    {
        private static TestDataManager testDataManager;
        private static AttorneyDataModel attorney;
        private static int attorneyKey;
        private static string attorneyContact;
        private static int legalEntityKey;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
         {
             fakeDb = new FakeDbFactory();
             testDataManager = new TestDataManager(fakeDb);
             legalEntityKey = 1235888;
             attorneyKey = 1235889;
             attorneyContact = "VishavP";
             attorney = new AttorneyDataModel(attorneyKey, attorneyContact, 567432.10, 1, 1500000, 2000000, true, legalEntityKey, true, (int)GeneralStatus.Active);
         };

        private Because of = () =>
         {
             testDataManager.InsertAttorney(attorney);
         };

        private It should_insert_the_attorney = () =>
         {
             fakeDb.FakedDb.InAppContext().
                WasToldTo(x => x.Insert(Arg.Is<AttorneyDataModel>(y => y.AttorneyContact == attorneyContact
                          && y.LegalEntityKey == legalEntityKey)));
         };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x=>x.Complete());
        };
    }
}
