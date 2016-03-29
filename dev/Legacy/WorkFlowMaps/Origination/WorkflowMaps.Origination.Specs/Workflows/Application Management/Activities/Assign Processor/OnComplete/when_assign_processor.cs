using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Assign_Processor.OnComplete
{
    [Subject("Activity => Assign_Processor => OnComplete")]
    internal class when_assign_processor : WorkflowSpecApplicationManagement
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
            result = workflow.OnCompleteActivity_Assign_Processor(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}