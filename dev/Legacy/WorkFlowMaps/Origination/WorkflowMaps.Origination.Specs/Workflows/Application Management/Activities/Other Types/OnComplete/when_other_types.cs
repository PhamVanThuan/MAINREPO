using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Other_Types.OnComplete
{
    [Subject("Activity => Other_Types => OnComplete")] // AutoGenerated
    internal class when_other_types : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            string message = string.Empty;
            result = workflow.OnCompleteActivity_Other_Types(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}