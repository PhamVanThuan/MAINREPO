using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Valuations_Request.OnStart
{
    [Subject("Activity => Valuations_Request => OnStart")] // AutoGenerated
    internal class when_valuations_request : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Valuations_Request(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}