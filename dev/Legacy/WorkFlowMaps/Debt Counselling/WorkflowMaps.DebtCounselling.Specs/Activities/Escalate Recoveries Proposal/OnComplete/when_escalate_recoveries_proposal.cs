using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._Escalate_Recoveries_Proposal.OnComplete
{
    [Subject("Activity => Escalate_Recoveries_Proposal => OnComplete")]
    internal class when_escalate_recoveries_proposal : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment workflowAssignment;

        private Establish context = () =>
        {
            result = false;
            workflowData.AssignWorkflowRoleTypeKey = (int)SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingSupervisorD;
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity__Escalate_Recoveries_Proposal(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_assign_the_workflow_role_for_the_aduser = () =>
        {
            workflowAssignment.WasToldTo(x => x.AssignWorkflowRoleForADUser((IDomainMessageCollection)messages, instanceData.ID, workflowData.AssignADUserName,
                SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingSupervisorD, workflowData.DebtCounsellingKey, ""));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}