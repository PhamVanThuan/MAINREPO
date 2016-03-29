using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using X2DomainService.Interface.DebtCounselling;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Decline.OnComplete
{
    [Subject("Activity => Decline => OnComplete")]
    internal class when_update_active_accepted_proposal_passes_and_assignment_passes : WorkflowSpecDebtCounselling
    {
        private static List<string> exclusionStates = new List<string> { "Debt Review Approved", "Bond Exclusions", "Bond Exclusions Arrears" };
        private static bool result;
        private static string message;
        private static IWorkflowAssignment wfa;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = false;
            workflowData.CourtCase = false;
            wfa = An<IWorkflowAssignment>();
            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            wfa.WhenToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                Param.IsAny<int>(), Param.IsAny<SAHL.Common.Globals.WorkflowRoleTypes>(), Param.IsAny<string>(),
                Param.IsAny<List<string>>(), Param.IsAny<bool>(), Param.IsAny<bool>())).Return(true);
            client.WhenToldTo(x => x.UpdateActiveAcceptedProposal(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>()))
                .Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Decline(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_assign_or_load_balance_to_the_debt_counselling_consultant = () =>
        {
            wfa.WasToldTo(x => x.AssignDebtCounsellingCaseForGroupOrLoadBalance((IDomainMessageCollection)messages, instanceData.ID, workflowData.DebtCounsellingKey,
                SAHL.Common.Globals.WorkflowRoleTypes.DebtCounsellingConsultantD, "Decline", exclusionStates, false, workflowData.CourtCase));
        };
    }
}