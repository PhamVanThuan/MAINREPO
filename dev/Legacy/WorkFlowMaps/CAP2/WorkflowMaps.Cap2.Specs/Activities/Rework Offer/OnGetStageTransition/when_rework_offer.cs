using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Rework_Offer.OnGetStageTransition
{
    [Subject("Activity => Rework_Offer => OnGetStageTransition")] // AutoGenerated
    internal class when_rework_offer : WorkflowSpecCap2
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Rework_Offer(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}