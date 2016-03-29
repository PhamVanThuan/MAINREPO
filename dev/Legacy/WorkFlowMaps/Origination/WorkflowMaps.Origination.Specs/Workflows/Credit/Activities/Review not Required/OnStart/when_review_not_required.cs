using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Review_not_Required.OnStart
{
    [Subject("Activity => Review_Not_Required => OnStart")]
    internal class when_review_not_required : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.ReviewRequired = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Review_not_Required(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldEqual(!workflowData.ReviewRequired);
        };
    }
}