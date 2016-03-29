using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.States.Archive_Further_Loan.OnEnter
{
    [Subject("State => Archive_Further_Loan => OnEnter")]
    internal class when_archive_further_loan : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static List<string> workflowRoles;

        private Establish context = () =>
        {
            result = false;
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            workflowRoles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D", };
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_Further_Loan(instanceData, workflowData, paramsData, messages);
        };

        private It should_deactivate_users_for_instance = () =>
        {
            workflowAssignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, workflowRoles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}