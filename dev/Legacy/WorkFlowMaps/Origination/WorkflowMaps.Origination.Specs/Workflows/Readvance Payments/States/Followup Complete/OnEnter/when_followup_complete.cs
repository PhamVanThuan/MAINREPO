using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Followup_Complete.OnEnter
{
    [Subject("State => Followup_Complete => OnEnter")]
    internal class when_followup_complete : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static List<string> workflowRoles;
        private static IWorkflowAssignment workflowAssignment;

        private Establish context = () =>
        {
            result = false;
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            workflowRoles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D" };
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Followup_Complete(instanceData, workflowData, paramsData, messages);
        };

        private It should_deactivate_users_for_instance_and_process = () =>
        {
            workflowAssignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, workflowRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}