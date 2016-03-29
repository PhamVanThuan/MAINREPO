using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Under_Cancellation_Removed.OnComplete
{
    [Subject("Activity => Under_Cancellation_Removed => OnComplete")]
    internal class when_under_cancellation_removed : WorkflowSpecDebtCounselling
    {
        private static string message;
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static List<string> states = new List<string>();

        private Establish context = () =>
            {
                workflowAssignment = An<IWorkflowAssignment>();
                result = false;
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
                states.Add("Debt Review Approved");
                workflowData.DebtCounsellingKey = 1;
                workflowData.CourtCase = false;
                ((InstanceDataStub)instanceData).ID = 1;
                workflowData.PreviousState = string.Empty;
                workflowAssignment.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance
                    ((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey, SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, workflowData.PreviousState, states, false, workflowData.CourtCase)).Return(true);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Under_Cancellation_Removed(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_load_balance_assign_to_the_debt_counselling_consultant = () =>
            {
                workflowAssignment.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance
                    ((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey, SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, workflowData.PreviousState, states, false, workflowData.CourtCase));
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };
    }
}