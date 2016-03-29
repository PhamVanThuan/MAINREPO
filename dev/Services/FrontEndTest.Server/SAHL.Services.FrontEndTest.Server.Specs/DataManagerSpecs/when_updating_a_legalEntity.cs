using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.FrontEndTest.Managers;
using Machine.Fakes;
using System;
using SAHL.Core.Testing.Fakes;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_updating_a_legalEntity
    {
        private static TestDataManager testDataManager;
        private static LegalEntityDataModel legalEntity;
        private static int LegalEntityKey;
        private static int OfferKey;
        private static string EmailAddress;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
        {
            LegalEntityKey = 123456789;
            OfferKey = 111;
            EmailAddress = "vishavp@sahomeloans.com";
            legalEntity = new LegalEntityDataModel((int)LegalEntityType.NaturalPerson, (int)MaritalStatus.Single, (int)Gender.Male
                                                   , (int)PopulationGroup.Unknown, DateTime.Now, 1, "My PC is", "MPI", "Premlall", "", "9209125163087", ""
                                                   , "", "", "", "", DateTime.Now, "031", "563", "5489", "", "0745847268", EmailAddress
                                                   , "", "", "HowUwishImadeThisPublic", 1, 1, "", 1, "", DateTime.Now, 3, 1, 1, 1);
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            testDataManager.UpdateClient(legalEntity);
        };

        private It should_update_the_correct_legalEntity = () =>
        {
            fakeDb.FakedDb.InAppContext().
                WasToldTo(x => x.Update(Arg.Is<LegalEntityDataModel>(y => y.LegalEntityKey == legalEntity.LegalEntityKey)));
        };

        private It should_complete_the_db_context = () =>
        {
            fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };
    }
}
