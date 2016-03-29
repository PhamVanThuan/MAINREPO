using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.ClientDomain.Managers;
using System.Collections.Generic;

namespace SAHL.Services.ClientDomain.Specs.Managers.ClientDataManagerSpecs
{
    public class when_finding_a_client_that_is_not_in_our_system : WithFakes
    {
        private static IClientDataManager clientDataManager;
        private static FakeDbFactory dbFactory;
        private static LegalEntityDataModel existingClientDataModel;
        private static LegalEntityDataModel results;
        private static int clientKey;

        private Establish context = () =>
        {
            clientKey = 22;
            dbFactory = new FakeDbFactory();
            clientDataManager = new ClientDataManager(dbFactory);

            existingClientDataModel = null;
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<LegalEntityDataModel>(Param.IsAny<ISqlStatement<LegalEntityDataModel>>()))
                .Return(existingClientDataModel);
        };

        private Because of = () =>
        {
            results = clientDataManager.FindExistingClient(clientKey);
        };

        private It should_query_for_an_existing_client = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<LegalEntityDataModel>(Param.IsAny<ISqlStatement<LegalEntityDataModel>>()));

        };

        private It should_not_return_the_client_details = () =>
        {
            results.ShouldBeNull();
        };
    }
}