using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Cancel_CAP2_Offer.OnGetActivityMessage
{
    [Subject("Activity => Cancel_CAP2_Offer => OnGetActivityMessage")] // AutoGenerated
    internal class when_cancel_cap2_offer : WorkflowSpecCap2
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Cancel_CAP2_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}