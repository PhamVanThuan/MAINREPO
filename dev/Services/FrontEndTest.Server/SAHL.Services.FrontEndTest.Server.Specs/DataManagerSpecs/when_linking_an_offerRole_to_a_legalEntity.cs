using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.FrontEndTest.Managers;
using SAHL.Core.Testing.Fakes;
using Machine.Fakes;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_linking_an_offerRole_to_a_legalEntity
    {
        private static OfferRoleDataModel offerRole;
        private static LegalEntityDataModel legalEntity;
        private static TestDataManager testDataManager;
        private static FakeDbFactory fakeDb;
        private static int LegalEntityKey;
        private static int OfferRoleKey;

        private Establish context = () =>
        {
            LegalEntityKey = 12091992;
            OfferRoleKey = 299129021;
            legalEntity = new LegalEntityDataModel((int)LegalEntityType.NaturalPerson, (int)MaritalStatus.Single, (int)Gender.Male
                                                     , (int)PopulationGroup.Unknown, DateTime.Now, 1, "My PC Is", "MPI", "Premlall", "", "9209125163087", ""
                                                     , "", "", "", "", DateTime.Now, "031", "563", "5489", "", "0745847268", "vishavp@sahomeloans.com"
                                                     , "", "", "HowUwishImadeThisPublic", 1, 1, "", 1, "", DateTime.Now, 3, 1, 1, 1);
            offerRole = new OfferRoleDataModel(LegalEntityKey, 1, OfferRoleKey, (int)GeneralStatus.Active, DateTime.Now);
            fakeDb = new FakeDbFactory();
            testDataManager = new TestDataManager(fakeDb);
        };

        private Because of = () =>
        {
            testDataManager.LinkClientToOffer(offerRole);
        };

        private It should_insert_the_correct_offerRole = () =>
        {
            fakeDb.FakedDb.InAppContext().
                   WasToldTo(x => x.Insert(Arg.Is<OfferRoleDataModel>(y=>y.OfferRoleKey == offerRole.OfferRoleKey)));
        };

        private It should_complete_the_db_context = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
         };
    }
}
