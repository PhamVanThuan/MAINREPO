using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Terminate_Application.OnComplete
{
    [Subject("Activity => Terminate_Application => OnComplete")]
    internal class when_terminating_application : WorkflowSpecDebtCounselling
    {
        private static string message;
        private static bool result;
        private static IWorkflowAssignment workflowAssignment;
        private static List<string> states = new List<string>();
        private static IDebtCounselling debtcounsellingClient;

        private Establish context = () =>
        {
            debtcounsellingClient = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(debtcounsellingClient);
            workflowAssignment = An<IWorkflowAssignment>();
            result = false;
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            states.Add("Bond Exclusions");
            states.Add("Bond Exclusions Arrears");
            workflowData.DebtCounsellingKey = 1;
            workflowData.CourtCase = false;
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.PreviousState = string.Empty;
            workflowAssignment.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance
                ((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey, SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, "Intent to Terminate", states, false, workflowData.CourtCase)).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Terminate_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_Post_973_Financial_Transaction = () =>
        {
            debtcounsellingClient.WasToldTo(x => x.RollbackTransaction((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey));
        };

        private It should_load_balance_assign_to_the_debt_counselling_consultant = () =>
        {
            workflowAssignment.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance
                ((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey, SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, "Intent to Terminate", states, false, workflowData.CourtCase));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}