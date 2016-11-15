using Machine.Specifications;

namespace WorkflowMaps.Cap2.Specs.Activities.LA_Sent.OnStart
{
    [Subject("Activity => LA_Sent => OnStart")] // AutoGenerated
    internal class when_la_sent : WorkflowSpecCap2
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_LA_Sent(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}