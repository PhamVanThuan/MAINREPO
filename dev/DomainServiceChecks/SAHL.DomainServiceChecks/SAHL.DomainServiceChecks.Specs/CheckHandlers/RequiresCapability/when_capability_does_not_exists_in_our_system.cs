﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.DomainServiceChecks.CheckHandlers;
using SAHL.DomainServiceChecks.Checks;
using SAHL.DomainServiceChecks.Managers.ClientDataManager;
using SAHL.DomainServiceChecks.Managers.WorkflowAssignmentDataManagerSpecs;
using System.Linq;

namespace SAHL.DomainService.Check.Specs.CheckHandlers.RequiresCapability
{
    public class when_capability_does_not_exists_in_our_system : WithFakes
    {
        private static IWorkflowAssignmentDataManager workflowAssignmentDataManager;
        private static IRequiresCapability check;
        private static RequiresCapabilityHandler handler;
        private static ISystemMessageCollection systemMessages;

        private Establish context = () =>
        {
            workflowAssignmentDataManager = An<IWorkflowAssignmentDataManager>();
            check = An<IRequiresCapability>();
            workflowAssignmentDataManager.WhenToldTo(x => x.DoesCapabilityExist(check.CapabilityKey)).Return(false);
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

        private It should_contain_error_mesages = () =>
        {
            systemMessages.ErrorMessages().First().Message.ShouldEqual("The capability provided does not exist.");
        };
    }
}