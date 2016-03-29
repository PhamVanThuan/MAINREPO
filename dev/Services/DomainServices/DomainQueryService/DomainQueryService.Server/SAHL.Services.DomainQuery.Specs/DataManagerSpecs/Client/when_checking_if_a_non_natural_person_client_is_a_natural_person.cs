using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.DomainQuery.Managers.Client;
using SAHL.Services.DomainQuery.Managers.Client.Statements;

namespace SAHL.DomainService.Check.Specs.Managers.Client
{
    public class when_checking_if_a_non_natural_person_client_is_a_natural_person : WithCoreFakes
    {
        private static ClientDataManager clientDataManager;
        private static int clientKey;
        private static bool isClientANaturalPersonResponse;
        private static FakeDbFactory fakeDbFactory;

        private Establish context = () =>
        {
            clientKey = 1;
            fakeDbFactory = new FakeDbFactory();
            clientDataManager = new ClientDataManager(fakeDbFactory);
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.SelectOne<int>(Param.IsAny<IsClientANaturalPersonStatement>())).Return((int)LegalEntityType.Company);
        };

        private Because of = () =>
        {
            isClientANaturalPersonResponse = clientDataManager.IsClientANaturalPerson(clientKey);
        };

        private It should_check_if_the_client_exits = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.SelectOne(Arg.Is<IsClientANaturalPersonStatement>(y => y.ClientKey == clientKey)));
        };

        private It should_acknolewdge_that_the_client_is_a_person = () =>
        {
            isClientANaturalPersonResponse.ShouldBeFalse();
        };
    }
}