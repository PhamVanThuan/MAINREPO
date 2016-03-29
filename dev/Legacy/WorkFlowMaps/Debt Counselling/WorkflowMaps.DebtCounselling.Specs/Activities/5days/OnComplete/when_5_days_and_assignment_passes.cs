using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities._5days.OnComplete
{
    [Subject("Activity => 5_Days => OnComplete")]
    internal class when_5_days_and_assignment_passes : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static string message;
        private static List<string> statesToExclude = new List<string>();

        private static ICommon common;
        private static IWorkflowAssignment workflowAssignment;
        private static IDebtCounselling debtcounsellingClient;

        private Establish context = () =>
        {
            debtcounsellingClient = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(debtcounsellingClient);

            result = false;
            message = string.Empty;
            workflowData.DebtCounsellingKey = 1;
            workflowData.CourtCase = false;
            ((InstanceDataStub)instanceData).ID = 1;

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);

            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            workflowAssignment.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(), Param.IsAny<List<string>>(),
                Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_5days(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_Post_973_Financial_Transaction = () =>
        {
            debtcounsellingClient.WasToldTo(x => x.RollbackTransaction((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey));
        };

        private It should_add_NoArrangementNotReferredToCourtAndNoPaymentsInTermsOfProposal_reason_to_debt_counselling_case = () =>
        {
            common.WasToldTo(x => x.AddReasons((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey,
                (int)SAHL.Common.Globals.ReasonDescriptions.NoArrangementNotReferredToCourtAndNoPaymentsInTermsOfProposal,
                (int)SAHL.Common.Globals.ReasonTypes.DebtCounsellingTermination));
        };

        private It should_assign_or_load_balance_the_case_to_the_debt_counselling_consultant = () =>
        {
            workflowAssignment.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey,
                SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, "Intent to Terminate", statesToExclude, false, workflowData.CourtCase));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}