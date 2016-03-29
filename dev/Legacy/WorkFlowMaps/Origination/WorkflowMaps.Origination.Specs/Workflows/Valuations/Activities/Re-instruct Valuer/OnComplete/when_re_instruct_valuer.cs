using Machine.Specifications;

namespace WorkflowMaps.Valuations.Specs.Activities.Re_instruct_Valuer.OnComplete
{
    [Subject("Activity => Re_instruct_Valuer => OnComplete")] // AutoGenerated
    internal class when_re_instruct_valuer : WorkflowSpecValuations
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Re_instruct_Valuer(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}