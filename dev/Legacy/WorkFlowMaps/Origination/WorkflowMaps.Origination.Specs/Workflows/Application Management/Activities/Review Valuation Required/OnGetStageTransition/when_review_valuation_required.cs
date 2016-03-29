using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Review_Valuation_Required.OnGetStageTransition
{
    [Subject("Activity => Review_Valuation_Required => OnGetStageTransition")]
    internal class when_review_valuation_required : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Review_Valuation_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_review_valuation_required_stagetransition = () =>
        {
            result.ShouldEqual<string>("Review Valuation Required");
        };
    }
}