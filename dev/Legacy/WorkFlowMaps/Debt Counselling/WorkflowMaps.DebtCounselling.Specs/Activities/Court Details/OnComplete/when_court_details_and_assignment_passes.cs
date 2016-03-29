using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Court_Details.OnComplete
{
    [Subject("Activity => Court_Details => OnComplete")]
    internal class when_court_details_and_assignment_passes : WorkflowSpecDebtCounselling
    {
        static bool result;
        static string message;
        static IWorkflowAssignment wfa;
        static List<string> exclusionStates = new List<string> { "Debt Review Approved", "Bond Exclusions", "Bond Exclusions Arrears" };
        Establish context = () =>
            {
                workflowData.CourtCase = false;
                result = false;
                wfa = An<IWorkflowAssignment>();
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
                wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                    Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(),
                    Param.IsAny<List<string>>(), Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
            };

        Because of = () =>
            {
                result = workflow.OnCompleteActivity_Court_Details(messages, workflowData, instanceData, paramsData, ref message);
            };

        It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        It should_assign_or_load_balance_to_the_debt_counselling_court_consultant = () =>
            {
                wfa.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(messages, instanceData.InstanceID, workflowData.DebtCounsellingKey,
                    SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingCourtConsultantD, "Court Details", exclusionStates, false, workflowData.CourtCase));
            };

        It should_set_court_case_data_property_to_true = () =>
            {
                workflowData.CourtCase.ShouldBeTrue();
            };
    }
}