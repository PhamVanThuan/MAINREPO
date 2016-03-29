using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.DomainQuery.Managers.Client;
using SAHL.Services.DomainQuery.Managers.Client.Statements;

namespace SAHL.Services.DomainQuery.Specs.DataManagerSpecs.Client
{
    public class when_checking_if_an_existing_client_exists : WithFakes
    {
        private static ClientDataManager clientDataManager;
        private static FakeDbFactory fakeDbFactory;
        private static bool result;
        private static int clientKey;

        private Establish context = () =>
        {
            clientKey = 12345;
            fakeDbFactory = new FakeDbFactory();
            clientDataManager = new ClientDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(
                x => x.SelectOne(Param.IsAny<ClientExistsStatement>())).Return(1);
        };

        private Because of = () =>
        {
            result = clientDataManager.IsClientOnOurSystem(clientKey);
        };

        private It should_use_the_client_key_in_the_statement = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(
                Arg.Is<ClientExistsStatement>(y => y.ClientKey == clientKey)));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}