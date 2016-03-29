using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.DomainQuery.Managers.Client;
using SAHL.Services.DomainQuery.QueryHandlers;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System.Linq;

namespace SAHL.Services.DomainQuery.Specs.QueryHandlerSpecs.DoesClientExist
{
    public class when_asked_for_a_client_not_on_our_systemd : WithFakes
    {
        private static IClientDataManager clientDataManager;
        private static DoesClientExistQueryHandler handler;
        private static DoesClientExistQuery query;
        private static int NaturalPersonClientClientKey;
        private static ISystemMessageCollection systemMessages;

        private Establish context = () =>
        {
            systemMessages = SystemMessageCollection.Empty();
            NaturalPersonClientClientKey = 99;
            clientDataManager = An<IClientDataManager>();
            clientDataManager.WhenToldTo(cm => cm.IsClientOnOurSystem(NaturalPersonClientClientKey)).Return(false);

            query = new DoesClientExistQuery(NaturalPersonClientClientKey);
            handler = new DoesClientExistQueryHandler(clientDataManager);
        };

        private Because of = () =>
        {
            systemMessages.Aggregate(handler.HandleQuery(query));
        };

        private It should_check_if_the_client_exists = () =>
        {
            clientDataManager.WasToldTo(c => c.IsClientOnOurSystem(NaturalPersonClientClientKey));
        };

        private It should_set_query_result_to_false = () =>
        {
            query.Result.Results.First().ClientExists.ShouldBeFalse();
        };

        private It should_not_add_system_messages = () =>
        {
            systemMessages.AllMessages.Any().ShouldBeFalse();
        };
    }
}