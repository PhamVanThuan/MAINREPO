using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Accepted_Proposal.OnEnter
{
    [Subject("State => Accepted_Proposal => OnEnter")]
    internal class when_accepted_proposal : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Accepted_Proposal(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}