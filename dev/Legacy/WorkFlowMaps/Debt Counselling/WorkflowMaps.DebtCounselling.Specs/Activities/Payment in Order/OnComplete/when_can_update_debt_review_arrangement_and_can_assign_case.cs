using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Payment_in_Order.OnComplete
{
    [Subject("Activity => Payment_in_Order => OnComplete")]
    internal class when_can_update_debt_review_arrangement_and_can_assign_case : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IDebtCounselling client;
        private static IWorkflowAssignment wfa;
        private static string message;
        private static List<string> exclusionStates = new List<string> { "Debt Review Approved", "Bond Exclusions", "Bond Exclusions Arrears" };

        private Establish context = () =>
            {
                result = true;
                workflowData.CourtCase = false;
                client = An<IDebtCounselling>();
                wfa = An<IWorkflowAssignment>();
                client.WhenToldTo(x => x.UpdateDebtCounsellingDebtReviewArrangement(Param.IsAny<IDomainMessageCollection>(),
                    Param.IsAny<int>(), Param.IsAny<string>())).Return(true);
                wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                        Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                        Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
                domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Payment_in_Order(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_assign_or_load_balance_the_case_to_the_debt_counselling_consultant = () =>
            {
                wfa.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey,
                        SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, "Debt Review Approved", exclusionStates, false, workflowData.CourtCase));
            };
    }
}