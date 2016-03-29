using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Managers.Client.Statements;

namespace SAHL.Services.ClientDomain.Specs.Managers.ClientDataManagerSpecs
{
    public class when_a_client_cannot_be_found_for_an_idnumber : WithFakes
    {
        private static ClientDataManager clientDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static LegalEntityDataModel existingClient;
        private static LegalEntityDataModel result;

        private Establish context = () =>
        {
            existingClient = null;
            fakeDbFactory = new FakeDbFactory();
            clientDataManager = new ClientDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo
             (x => x.SelectOne(Param.IsAny<FindClientByIdNumberStatement>())).Return(existingClient);
        };

        private Because of = () =>
        {
            result = clientDataManager.FindExistingClientByIdNumber("8211045229080");
        };

        private It should_return_null = () =>
        {
            result.ShouldEqual(null);
        };
    }
}