using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Attorney_to_Oppose.OnComplete
{
    [Subject("Activity => Attorney_To_Oppose => OnComplete")]
    internal class when_attorney_to_oppose_and_assignment_passes : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static List<string> statesToExclude = new List<string> { "Debt Review Approved", "Bond Exclusions", "Bond Exclusions Arrears" };
        private static IWorkflowAssignment workflowAssignment;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.CourtCase = false;

            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            workflowAssignment.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Attorney_to_Oppose(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_the_court_case_data_property = () =>
        {
            workflowData.CourtCase.ShouldBeTrue();
        };

        private It should_assign_or_load_balance_the_case_to_the_debt_counselling_court_consultant = () =>
        {
            workflowAssignment.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey,
                SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, "Attorney to Oppose", statesToExclude, false, workflowData.CourtCase));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}