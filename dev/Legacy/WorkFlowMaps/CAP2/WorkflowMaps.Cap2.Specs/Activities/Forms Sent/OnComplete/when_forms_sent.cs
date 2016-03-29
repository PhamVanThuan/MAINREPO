using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Forms_Sent.OnComplete
{
    [Subject("Activity => Forms_Sent => OnComplete")]
    internal class when_forms_sent : WorkflowSpecCap2
    {
        private static string message;
        private static bool result;

        private Establish context = () =>
        {
            message = string.Empty;
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Forms_Sent(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}