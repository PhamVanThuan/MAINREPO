using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.DomainQuery.Managers.Client;
using SAHL.Services.DomainQuery.QueryHandlers;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System.Linq;

namespace SAHL.Services.DomainQuery.Specs.QueryHandlerSpecs.IsClientANaturalPerson
{
    public class when_checking_if_a_natural_person_client_is_a_natural_person : WithCoreFakes
    {
        private static IClientDataManager clientDataManager;
        private static IsClientANaturalPersonQueryHandler handler;
        private static IsClientANaturalPersonQuery query;
        private static int NaturalPersonClientClientKey;
        private static ISystemMessageCollection systemMessages;

        private Establish context = () =>
        {
            systemMessages = SystemMessageCollection.Empty();
            NaturalPersonClientClientKey = 1001;
            clientDataManager = An<IClientDataManager>();
            clientDataManager.WhenToldTo(cm => cm.IsClientANaturalPerson(NaturalPersonClientClientKey)).Return(true);

            query = new IsClientANaturalPersonQuery(NaturalPersonClientClientKey);
            handler = new IsClientANaturalPersonQueryHandler(clientDataManager);
        };

        private Because of = () =>
        {
            systemMessages.Aggregate(handler.HandleQuery(query));
        };

        private It should_check_if_a_client_is_a_natural_person = () =>
        {
            clientDataManager.WasToldTo(c => c.IsClientANaturalPerson(NaturalPersonClientClientKey));
        };

        private It should_acknowledge_that_client_is_a_natural_person = () =>
        {
            query.Result.Results.First().ClientIsANaturalPerson.ShouldBeTrue();
        };

        private It should_not_add_system_messages = () =>
        {
            systemMessages.AllMessages.Any().ShouldBeFalse();
        };
    }
}