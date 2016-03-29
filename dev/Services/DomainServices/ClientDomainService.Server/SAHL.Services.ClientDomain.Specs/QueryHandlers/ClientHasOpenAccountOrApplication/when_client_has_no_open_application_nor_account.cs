using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.QueryHandlers;
using SAHL.Services.Interfaces.ClientDomain.Queries;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.QueryHandlers.ClientHasOpenAccountOrApplication
{
    public class when_client_has_no_open_application_nor_account : WithFakes
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

            clientDataManager.WhenToldTo(cdm => cdm.FindOpenApplicationNumbersForClient(Param.IsAny<int>()))
            .Return(
                new int[] { }
            );

            clientDataManager.WhenToldTo(cdm => cdm.FindOpenAccountKeysForClient(Param.IsAny<int>()))
            .Return(
                new int[] { }
            );
        };

        private Because of = () =>
        {
            systemMessages = queryHandler.HandleQuery(query);
        };

        private It should_check_for_an_open_application = () =>
        {
            clientDataManager.WasToldTo(cdm => cdm.FindOpenApplicationNumbersForClient(Param.Is<int>(clientKey)));
        };

        private It should_check_for_an_open_account = () =>
        {
            clientDataManager.WasToldTo(cdm => cdm.FindOpenAccountKeysForClient(Param.Is<int>(clientKey)));
        };

        private It should_not_add_any_system_message = () =>
        {
            systemMessages.AllMessages.Any().ShouldBeFalse();
        };

        private It should_confirm_absence_of_both_an_account_and_an_application = () =>
        {
            query.Result.Results.Any(rslt => rslt.ClientIsAlreadyAnSAHLCustomer).ShouldBeFalse();
        };
    }
}