using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Review_Valuation_Required.OnGetActivityMessage
{
    [Subject("Activity => Review_Valuation_Required => OnGetActivityMessage")] // AutoGenerated
    internal class when_review_valuation_required : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Review_Valuation_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}