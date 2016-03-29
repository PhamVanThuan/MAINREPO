using Machine.Specifications;
using WorkflowMaps.Valuations.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Request_Valuation_Review.OnComplete
{
    [Subject("Activity => Request_Valuation_Review => OnComplete")]
    internal class when_request_valuation_review : WorkflowSpecValuations
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            workflowData.IsReview = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Request_Valuation_Review(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_is_review_data_property_to_true = () =>
        {
            workflowData.IsReview.ShouldBeTrue();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}