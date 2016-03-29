using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.DebtCounselling;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Accept.OnStart
{
    [Subject("Activity => Accept => OnStart")]
    internal class when_accept_and_rules_fail : WorkflowSpecDebtCounselling
    {
        private static bool result;
        private static IDebtCounselling client;

        private Establish context = () =>
        {
            result = true;
            workflowData.DebtCounsellingKey = 1;

            client = An<IDebtCounselling>();
            domainServiceLoader.RegisterMockForType<IDebtCounselling>(client);
            client.WhenToldTo(x => x.CheckAcceptProposalRules(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>(), Param.IsAny<bool>())).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Accept(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_the_accept_proposal_rules = () =>
        {
            client.WasToldTo(x => x.CheckAcceptProposalRules((IDomainMessageCollection)messages, workflowData.DebtCounsellingKey, paramsData.IgnoreWarning));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}