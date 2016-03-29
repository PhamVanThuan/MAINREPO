using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FrontEndTest.Managers;
using System;

namespace SAHL.Services.FrontEndTest.Server.Specs.DataManagerSpecs
{
    public class when_creating_a_legalentity
    {
        private static TestDataManager testDataManager;
        private static LegalEntityDataModel legalEntity;
        private static FakeDbFactory fakeDb;

        private Establish context = () =>
          {
              legalEntity = new LegalEntityDataModel((int)LegalEntityType.NaturalPerson, (int)MaritalStatus.Single, (int)Gender.Male
                                                     , (int)PopulationGroup.Unknown, DateTime.Now, 1, "My PC is", "MPI", "Premlall", "", "9209125163087", ""
                                                     , "", "", "", "", DateTime.Now, "031", "563", "5489", "", "0745847268", "vishavp@sahomeloans.com"
                                                     , "", "", "HowUwishImadeThisPublic", 1, 1, "", 1, "", DateTime.Now, 3, 1, 1, 1);
              fakeDb = new FakeDbFactory();
              testDataManager = new TestDataManager(fakeDb);
          };

        private Because of = () =>
          {
              testDataManager.InsertClient(legalEntity);
          };

        private It should_insert_the_correct_legalEntity = () =>
         {
             fakeDb.FakedDb.InAppContext().
                 WasToldTo(x => x.Insert(Arg.Is<LegalEntityDataModel>(y => y.IDNumber == legalEntity.IDNumber)));
         };

        private It should_complete_the_db_context = () =>
         {
             fakeDb.FakedDb.InAppContext().WasToldTo(x => x.Complete());
         };
    }
}