using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_updating_a_third_party
    {
        private static LegalEntityDataModel thirdParty;
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
         {
             fakeDb = new FakeDbFactory();
             testDataManager = new TestDataManager(fakeDb);
             thirdParty  = new LegalEntityDataModel((int)LegalEntityType.Company, (int)MaritalStatus.Single, (int)Gender.Male
                                                     , (int)PopulationGroup.Unknown, DateTime.Now, 1, "My PC is", "MPIP", "Premlall", "", "9209125163087", ""
                                                     , "", "", "", "", DateTime.Now, "031", "563", "5489", "", "0745847268", "vishavp@sahomeloans.com"
                                                     , "", "", "HowUwishImadeThisPublic", 1, 1, "", 1, "", DateTime.Now, 3, 1, 1, 1);
         };

        private Because of = () =>
         {
             testDataManager.UpdateThirdParty(thirdParty);
         };

        private It should_update_the_third_party = () =>
         {
             fakeDb.FakedDb.InAppContext().
                WasToldTo(x => x.Update(Arg.Is<LegalEntityDataModel>(y => y.FirstNames == "My PC is"
                                                                       && y.IDNumber == "9209125163087")));
         };

        private It should_complete_the_db_context = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
         };
    }
}
