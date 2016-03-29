using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.States.Intent_to_Terminate.OnEnter
{
    [Subject("State => Intent_to_Terminate => OnEnter")]
    internal class when_intent_to_terminate : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static bool expectedResult;
        private static List<string> expectedStatesToExclude;
        private static IWorkflowAssignment assignment;

        private Establish context = () =>
        {
            result = false;
            workflowData.CourtCase = false;
            expectedResult = true;
            expectedStatesToExclude = new List<string>() { "Debt Review Approved" };
            assignment = An<IWorkflowAssignment>();
            assignment.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<int>(),
                Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                Param.IsAny<bool>(), Param.IsAny<bool>()))
                .Return(expectedResult);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Intent_to_Terminate(instanceData, workflowData, paramsData, messages);
        };

        private It should_assign_debt_counselling_case_for_group_or_load_balance_to_debt_counselling_consultant_d = () =>
        {
            assignment.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey, SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, "Intent to Terminate", expectedStatesToExclude, false, workflowData.CourtCase));
        };

        private It should_return_assign_debt_counselling_case_for_group_or_load_balance_return_value = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}