using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress;
using SAHL.Services.ClientDomain.Managers.DomiciliumAddress.Statements;

namespace SAHL.Services.ClientDomain.Specs.Managers.DomiciliumAddressDataManagerSpecs
{
    public class when_checking_if_client_address_is_active_domicilium_on_client : WithFakes
    {
        private static IDomiciliumAddressDataManager domiciliumAddressManager;
        private static ClientAddressIsActiveDomiciliumAddressOnClientStatement query;
        private static int clientAddressKey;
        private static int clientKey;
        private static bool result;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            clientKey = 1;
            clientAddressKey = 12345;
            dbFactory = new FakeDbFactory();
            domiciliumAddressManager = new DomiciliumAddressDataManager(dbFactory);
            query = new ClientAddressIsActiveDomiciliumAddressOnClientStatement(clientAddressKey, clientKey);
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne<int>(Param.IsAny<ClientAddressIsActiveDomiciliumAddressOnClientStatement>()))
                .Return(1);
        };

        private Because of = () =>
        {
            result = domiciliumAddressManager.IsClientAddressActiveDomicilium(clientAddressKey, clientKey);
        };

        private It should_check_for_client_address = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<int>(Param.IsAny<ClientAddressIsActiveDomiciliumAddressOnClientStatement>()));
        };

        private It should_find_client_address = () =>
        {
            result.ShouldBeTrue();
        };
    }
}