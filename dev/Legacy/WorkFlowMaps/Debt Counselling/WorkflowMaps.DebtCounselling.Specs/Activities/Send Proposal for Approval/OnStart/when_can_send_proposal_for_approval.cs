using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Send_Proposal_for_Approval.OnStart
{
    internal class when_can_send_proposal_for_approval : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = true;
            client = An<IDebtCounselling>();
            client.WhenToldTo(x => x.CheckSendProposalForApprovalRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(),
                Param.IsAny<bool>())).Return(true);
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Send_Proposal_for_Approval(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}