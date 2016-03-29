using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.Forms_Sent.OnStart
{
    [Subject("Activity => Forms_Sent => OnStart")]
    internal class when_forms_sent : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Forms_Sent(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}