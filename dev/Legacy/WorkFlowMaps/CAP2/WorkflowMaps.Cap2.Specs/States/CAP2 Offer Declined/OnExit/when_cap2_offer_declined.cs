using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.States.CAP2_Offer_Declined.OnExit
{
    [Subject("State => CAP2_Offer_Declined => OnExit")] // AutoGenerated
    internal class when_cap2_offer_declined : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnExit_CAP2_Offer_Declined(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}