using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.QueryHandlers;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.QueryHandlers.ClientHasOpenAccountOrApplication
{
    public class when_client_has_an_open_account : WithFakes
    {
        private static IClientDataManager clientDataManager;
        private static ClientHasOpenAccountOrApplicationQuery query;
        private static ClientHasOpenAccountOrApplicationQueryHandler queryHandler;
        private static ISystemMessageCollection systemMessages;
        private static int clientKey;

        private Establish context = () =>
        {
            clientKey = 234;
            clientDataManager = An<IClientDataManager>();
            query = new ClientHasOpenAccountOrApplicationQuery(clientKey);

            systemMessages = SystemMessageCollection.Empty();
            queryHandler = new ClientHasOpenAccountOrApplicationQueryHandler(clientDataManager);

            clientDataManager.WhenToldTo(cdm => cdm.FindOpenAccountKeysForClient(Param.IsAny<int>()))
            .Return(
                new int[] { 48378 }
            );

            clientDataManager.WhenToldTo(cdm => cdm.FindOpenApplicationNumbersForClient(Param.IsAny<int>()))
           .Return(
               new int[] { }
           );
        };

        private Because of = () =>
        {
            systemMessages = queryHandler.HandleQuery(query);
        };

        private It should_check_for_open_application = () =>
        {
            clientDataManager.WasToldTo(cdm => cdm.FindOpenApplicationNumbersForClient(Param.Is<int>(clientKey)));
        };

        private It should_check_for_open_accounts = () =>
        {
            clientDataManager.WasToldTo(cdm => cdm.FindOpenAccountKeysForClient(Param.Is<int>(clientKey)));
        };

        private It should_confirm_presence_of_an_open_account = () =>
        {
            query.Result.Results.Any(rslt => rslt.ClientIsAlreadyAnSAHLCustomer).ShouldBeTrue();
        };

        private It should_not_add_any_system_message = () =>
        {
            systemMessages.AllMessages.Any().ShouldBeFalse();
        };
    }
}