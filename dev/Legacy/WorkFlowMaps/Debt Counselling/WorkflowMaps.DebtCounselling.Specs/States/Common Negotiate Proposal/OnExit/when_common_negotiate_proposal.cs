using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.States.Common_Negotiate_Proposal.OnExit
{
    [Subject("State => Common_Negotiate_Proposal => OnExit")] // AutoGenerated
    internal class when_common_negotiate_proposal : WorkflowSpecDebtCounselling
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_Common_Negotiate_Proposal(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}