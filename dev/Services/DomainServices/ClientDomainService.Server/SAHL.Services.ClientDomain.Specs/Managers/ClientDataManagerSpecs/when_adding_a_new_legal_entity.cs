using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ClientDomain.Managers;
using System;

namespace SAHL.Services.ClientDomain.Specs.Managers.ClientDataManagerSpecs
{
    public class when_adding_a_new_legal_entity : WithFakes
    {
        private static IClientDataManager clientDataManager;
        private static LegalEntityDataModel naturalPersonClient;
        private static int clientKey;
        private static int result;
        private static FakeDbFactory fakedDb;

        private Establish context = () =>
        {
            fakedDb = new FakeDbFactory();
            clientDataManager = new ClientDataManager(fakedDb);
            clientKey = 77;
            naturalPersonClient = new LegalEntityDataModel(null, null, null, null, DateTime.Now, null, "Bob", "BB", "Builder", "", "1234567890123", "", ""
                , "", "", "", null, "", "", "", "", "", "", "", "", "", null, null, "", null, "", null, null, null, 1, null);
            fakedDb.FakedDb.InAppContext().WhenToldTo(x => x.Insert<LegalEntityDataModel>(naturalPersonClient)).Callback<LegalEntityDataModel>(m => m.LegalEntityKey = clientKey);
        };

        private Because of = () =>
        {
            result = clientDataManager.AddNewLegalEntity(naturalPersonClient);
        };

        private It should_save_the_legal_entity = () =>
        {
            fakedDb.FakedDb.InAppContext().WasToldTo(x => x.Insert<LegalEntityDataModel>(naturalPersonClient));
        };

        private It should_return_the_client_key = () =>
        {
            result = clientKey;
        };
    }
}