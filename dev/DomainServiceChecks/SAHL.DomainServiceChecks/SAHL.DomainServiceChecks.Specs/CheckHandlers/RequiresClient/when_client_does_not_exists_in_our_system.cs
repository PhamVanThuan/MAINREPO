using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ClientDataManager;
using System.Linq;

namespace SAHL.DomainService.Check.Specs.CheckHandlers.RequiresClient
{
    public class when_client_does_not_exists_in_our_system : WithFakes
    {
        private static IClientDataManager clientDataManager;
        private static IRequiresClient check;
        private static RequiresClientCheckHandler handler;
        private static ISystemMessageCollection systemMessages;

        private Establish context = () =>
        {
            clientDataManager = An<IClientDataManager>();
            check = An<IRequiresClient>();
            clientDataManager.WhenToldTo(x => x.IsClientOnOurSystem(check.ClientKey)).Return(false);
            handler = new RequiresClientCheckHandler(clientDataManager);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(check);
        };

        private It should_check_if_the_client_exists_in_our_system = () =>
        {
            clientDataManager.WasToldTo(x => x.IsClientOnOurSystem(check.ClientKey));
        };

        private It should_contain_error_mesages = () =>
        {
            systemMessages.ErrorMessages().First().Message.ShouldEqual("The client provided, does not exist.");
        };
    }
}