using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.ClientDataManager;
using SAHL.DomainServiceChecks.Managers.ClientDataManager.Statements;

namespace SAHL.DomainService.Check.Specs.Managers.Client
{
    public class when_checking_for_a_client_that_is_in_our_system : WithFakes
    {
        private static ClientDataManager clientDataManager;
        private static int clientKey;
        private static bool clientExistsResponse;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            fakeDbFactory = new FakeDbFactory();
            clientKey = 1;
            clientDataManager = new ClientDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<int>(Param.IsAny<ClientExistsStatement>())).Return(1);
        };

        private Because of = () =>
        {
            clientExistsResponse = clientDataManager.IsClientOnOurSystem(clientKey);
        };

        private It should_check_if_the_client_exits = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<ClientExistsStatement>(y => y.ClientKey == clientKey)));
        };

        private It should_acknolewdge_that_the_client_exists_in_our_system = () =>
        {
            clientExistsResponse.ShouldBeTrue();
        };
    }
}