using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Print_Cap_Letter.OnComplete
{
    [Subject("Activity => Print_Cap_Letter => OnComplete")]
    internal class when_printing_cap_letter : WorkflowSpecCap2
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Print_Cap_Letter(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}