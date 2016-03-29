using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Inadequate_Payment.OnComplete
{
    [Subject("Activity => Inadequate_Payment => OnComplete")]
    internal class when_payment_is_inadequate_and_assignment_passes : WorkflowSpecDebtCounselling
    {
        private static IWorkflowAssignment wfa;
        private static string message;
        private static bool result;
        private static List<string> exclusionStates = new List<string> { "Debt Review Approved", "Bond Exclusions", "Bond Exclusions Arrears" };

        private Establish context = () =>
            {
                result = false;
                workflowData.CourtCase = false;
                wfa = An<IWorkflowAssignment>();
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
                wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                    Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                    Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
                workflowData.DebtCounsellingKey = 1;
                ((InstanceDataStub)instanceData).ID = 1;
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Inadequate_Payment(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_assign_or_load_balance_to_the_debt_counselling_consultant = () =>
            {
                wfa.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey,
                    SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, "Pend Payment", exclusionStates, false, workflowData.CourtCase));
            };
    }
}