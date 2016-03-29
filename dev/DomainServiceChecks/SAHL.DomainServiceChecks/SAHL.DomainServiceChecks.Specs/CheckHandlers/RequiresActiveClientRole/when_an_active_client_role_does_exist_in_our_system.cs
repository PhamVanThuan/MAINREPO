using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ApplicationDataManager;
using System.Linq;

namespace SAHL.DomainServiceCheck.Specs.CheckHandlers.RequiresActiveClientRole
{
    public class when_an_active_client_role_does_exist_in_our_system : WithFakes
    {
        private static IApplicationDataManager applicationDataManager;
        private static IRequiresActiveClientRole check;
        private static RequiresActiveClientRoleHandler handler;
        private static ISystemMessageCollection systemMessages;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            check = An<IRequiresActiveClientRole>();
            applicationDataManager.WhenToldTo(x => x.IsActiveClientRole(check.ApplicationRoleKey)).Return(true);
            handler = new RequiresActiveClientRoleHandler(applicationDataManager);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(check);
        };

        private It should_check_if_the_client_exists_in_our_system = () =>
        {
            applicationDataManager.WasToldTo(x => x.IsActiveClientRole(check.ApplicationRoleKey));
        };

        private It should_not_contain_error_mesages = () =>
        {
            systemMessages.ErrorMessages().Count().ShouldEqual(0);
        };
    }
}