using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Decline___Offer.OnStart
{
    [Subject("Activity => Decline_+_Offer => OnStart")]
    internal class when_decline__offer : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.ActionSource = "Decline with Offer";
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Decline___Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}