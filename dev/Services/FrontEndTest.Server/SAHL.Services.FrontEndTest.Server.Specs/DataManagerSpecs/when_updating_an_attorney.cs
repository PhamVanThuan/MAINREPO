using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_updating_an_attorney
    {
        private static TestDataManager testDataManager;
        private static AttorneyDataModel attorney;
        private static string attorneyContact;
        private static int legalEntityKey;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
         {
             attorneyContact = "VishavP";
             legalEntityKey = 1235888;
             fakeDb = new FakeDbFactory();
             testDataManager = new TestDataManager(fakeDb);
             attorney = new AttorneyDataModel(1, attorneyContact, 12345.67, 1, 10000000.00, 2000000.00,
                                              true, legalEntityKey, true, (int)GeneralStatus.Active);
         };

        private Because of = () =>
         {
             testDataManager.UpdateAttorney(attorney);
         };

        private It should_update_the_correct_attorney_with_the_correct_details = () =>
         {
             fakeDb.FakedDb.InAppContext().
                WasToldTo(x => x.Update(Arg.Is<AttorneyDataModel>(y => y.LegalEntityKey == legalEntityKey
                          && y.AttorneyContact == attorneyContact && y.GeneralStatusKey == attorney.GeneralStatusKey)));
         };

        private It should_complete_the_db_context = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
         };
    }
}