using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.DomainQuery.Managers.Client;
using SAHL.Services.DomainQuery.QueryHandlers;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Linq;

namespace SAHL.Services.DomainQuery.Specs.QueryHandlerSpecs.DoesClientExist
{
    public class when_asked_for_a_client_on_our_system : WithFakes
    {
        private static IClientDataManager clientDataManager;
        private static DoesClientExistQueryHandler handler;
        private static DoesClientExistQuery query;
        private static int existingClientKey;
        private static ISystemMessageCollection systemMessages;

        private Establish context = () =>
        {
            systemMessages = SystemMessageCollection.Empty();
            existingClientKey = 3;
            clientDataManager = An<IClientDataManager>();
            clientDataManager.WhenToldTo(cm => cm.IsClientOnOurSystem(existingClientKey)).Return(true);

            query = new DoesClientExistQuery(existingClientKey);
            handler = new DoesClientExistQueryHandler(clientDataManager);
        };

        private Because of = () =>
        {
            systemMessages.Aggregate(handler.HandleQuery(query));
        };

        private It should_check_if_the_client_exists = () =>
        {
            clientDataManager.WasToldTo(c => c.IsClientOnOurSystem(existingClientKey));
        };

        private It should_set_query_result = () =>
        {
            query.Result.Results.First().ClientExists.ShouldBeTrue();
        };

        private It should_not_add_any_system_messages = () =>
        {
            systemMessages.AllMessages.Any().ShouldBeFalse();
        };
    }
}