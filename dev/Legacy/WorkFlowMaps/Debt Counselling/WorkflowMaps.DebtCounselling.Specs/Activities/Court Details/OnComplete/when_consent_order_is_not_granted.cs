using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using SAHL.Core.X2;
using System.Collections.Generic;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Court_Details.OnComplete
{
    [Subject("Activity => Court_Details => OnComplete")]
    internal class when_consent_order_is_not_granted : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment wfa;
        private static IX2Params parameters;
        private static List<string> exclusionStates = new List<string> { "Debt Review Approved", "Bond Exclusions", "Bond Exclusions Arrears" };

        private Establish context = () =>
            {
                parameters = An<IX2Params>();
                parameters.WhenToldTo(x => x.Data).Return((int)SAHL.Common.Globals.HearingAppearanceTypes.Appeal);
                result = false;
                wfa = An<IWorkflowAssignment>();
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
                wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                    Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(),
                    Param.IsAny<List<string>>(), Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Court_Details(instanceData, workflowData, parameters, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_assign_or_load_balance_to_the_debt_counselling_court_consultant = () =>
            {
                wfa.WasNotToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey,
                    SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, "Court Details", exclusionStates, true, workflowData.CourtCase));
            };

        private It should_set_court_case_data_property_to_true = () =>
        {
            workflowData.CourtCase.ShouldBeTrue();
        };
    }
}