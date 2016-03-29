using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ClientDomain.Managers;
using System;

namespace SAHL.Services.ClientDomain.Specs.Managers.ClientDataManagerSpecs
{
    public class when_updating_an_existing_legal_entity : WithFakes
    {
        private static IClientDataManager clientDataManager;
        private static LegalEntityDataModel naturalPersonClient;
        private static int clientKey;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            clientDataManager = new ClientDataManager(dbFactory);

            clientKey = 77;
            naturalPersonClient = new LegalEntityDataModel
             (clientKey, null, null, null, null, DateTime.Now, null, "Bob", "BB", "Builder", "", "1234567890123", ""
              , "", "", "", "", null, "", "", "", "", "", "", "", "", "", null, null, "", null, "", null, null, null, 1, null);
        };

        private Because of = () =>
        {
            clientDataManager.UpdateLegalEntity(naturalPersonClient);
        };

        private It should_save_the_legal_entity = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo
             (x => x.Update
              <LegalEntityDataModel>(naturalPersonClient));
        };
    }
}