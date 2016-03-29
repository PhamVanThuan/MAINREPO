using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ClientDataManager;
using SAHL.DomainServiceChecks.Managers.WorkflowAssignmentDataManagerSpecs;
using System.Linq;

namespace SAHL.DomainService.Check.Specs.CheckHandlers.RequiresCapability
{
    public class when_capability_exists : WithFakes
    {
        private static IWorkflowAssignmentDataManager workflowAssignmentDataManager;
        private static IRequiresCapability check;
        private static RequiresCapabilityHandler handler;
        private static ISystemMessageCollection systemMessages;

        private Establish context = () =>
        {
            workflowAssignmentDataManager = An<IWorkflowAssignmentDataManager>();
            check = An<IRequiresCapability>();
            workflowAssignmentDataManager.WhenToldTo(x => x.DoesCapabilityExist(check.CapabilityKey)).Return(true);
            handler = new RequiresCapabilityHandler(workflowAssignmentDataManager);
        };

        private Because of = () =>
        {
            systemMessages = handler.HandleCheckCommand(check);
        };

        private It should_check_if_the_client_exists_in_our_system = () =>
        {
            workflowAssignmentDataManager.WasToldTo(x => x.DoesCapabilityExist(check.CapabilityKey));
        };

        private It should_not_return_error_mesages = () =>
        {
            systemMessages.AllMessages.Any().ShouldBeFalse();
        };
    }
}