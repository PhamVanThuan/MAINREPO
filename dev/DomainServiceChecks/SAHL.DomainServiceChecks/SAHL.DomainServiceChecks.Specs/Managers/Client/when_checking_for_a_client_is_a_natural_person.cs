using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainServiceChecks.Managers.ClientDataManager;
using SAHL.DomainServiceChecks.Managers.ClientDataManager.Statements;

namespace SAHL.DomainServiceCheck.Specs.Managers.Client
{
    public class when_checking_for_a_client_is_a_natural_person : WithFakes
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
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<int>(Param.IsAny<IsClientANaturalPersonStatement>())).Return(
                (int)LegalEntityType.NaturalPerson);
        };

        private Because of = () =>
        {
            clientExistsResponse = clientDataManager.IsClientANaturalPerson(clientKey);
        };

        private It should_check_if_the_client_is_a_natural_person = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<IsClientANaturalPersonStatement>(y => y.ClientKey == clientKey)));
        };

        private It should_return_true = () =>
        {
            clientExistsResponse.ShouldBeTrue();
        };
    }
}