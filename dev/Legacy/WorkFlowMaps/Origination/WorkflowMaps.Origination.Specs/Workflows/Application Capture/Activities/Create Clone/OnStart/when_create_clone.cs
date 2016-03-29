using Machine.Specifications;
using System;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.Create_Clone.OnStart
{
    [Subject("Activity => Create_Clone => OnStart")]
    internal class when_create_clone : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.Last_State = String.Empty;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Create_Clone(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}