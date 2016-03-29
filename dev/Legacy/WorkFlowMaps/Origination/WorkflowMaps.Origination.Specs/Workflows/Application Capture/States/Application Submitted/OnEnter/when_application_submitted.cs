using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.States.Application_Submitted.OnEnter
{
    [Subject("State => Application_Submitted => OnEnter")]
    internal class when_application_submitted : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static List<string> workflowRoles;
        private static IWorkflowAssignment workflowAssignment;
        private static IApplicationCapture applicationCapture;

        private Establish context = () =>
        {
            result = false;
            workflowData.ApplicationKey = 1;

            applicationCapture = An<IApplicationCapture>();

            workflowRoles = new List<string>();
            workflowRoles.Add(SAHL.Common.ApplicationCapture.WorkflowRoles.BranchConsultantD);
            workflowRoles.Add(SAHL.Common.ApplicationCapture.WorkflowRoles.BranchAdminD);
            workflowRoles.Add(SAHL.Common.ApplicationCapture.WorkflowRoles.BranchManagerD);

            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(applicationCapture);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Application_Submitted(instanceData, workflowData, paramsData, messages);
        };

        private It should_send_an_sms_to_the_client_with_the_account_number_and_consultant_name = () =>
            {
                applicationCapture.WasToldTo(x => x.SendNewClientConsultantDetailsSMS(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()));
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