using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.States.Application_Not_Accepted.OnEnter
{
    [Subject("State => Application_Not_Accepted => OnEnter")]
    internal class when_application_not_accepted : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static List<string> workflowRoles;
        private static ICommon common;
        private static IWorkflowAssignment workflowAssignment;

        private Establish context = () =>
        {
            result = false;
            workflowData.ApplicationKey = 1;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);

            workflowRoles = new List<string>();
            workflowRoles.Add(SAHL.Common.ApplicationCapture.WorkflowRoles.BranchConsultantD);
            workflowRoles.Add(SAHL.Common.ApplicationCapture.WorkflowRoles.BranchAdminD);
            workflowRoles.Add(SAHL.Common.ApplicationCapture.WorkflowRoles.BranchManagerD);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Application_Not_Accepted(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_the_offer_status_to_NTU = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, (int)SAHL.Common.Globals.OfferStatuses.NTU, -1));
        };

        private It should_deactivate_the_users_for_this_instance = () =>
        {
            workflowAssignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey,
                workflowRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}