using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Court_Details.OnComplete
{
    [Subject("Activity => Court_Details => OnComplete")]
    internal class when_case_gets_assigned : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment wfa;
        private static List<string> exclusionStates = new List<string> { "Debt Review Approved", "Bond Exclusions", "Bond Exclusions Arrears" };

        private Establish context = () =>
            {
                workflowData.CourtCase = false;
                result = false;
                wfa = An<IWorkflowAssignment>();
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
                wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                    Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(),
                    Param.IsAny<List<string>>(), Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Court_Details(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_assign_or_load_balance_to_the_debt_counselling_court_consultant = () =>
            {
                wfa.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey,
                    SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, "Court Details", exclusionStates, false, workflowData.CourtCase));
            };

        private It should_set_court_case_data_property_to_true = () =>
            {
                workflowData.CourtCase.ShouldBeTrue();
            };
    }
}