using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Return_Processor_Readvance.OnComplete
{
    [Subject("Activity => Return_Processor_Readvance => OnComplete")]
    internal class when_return_processor_readvance : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Return_Processor_Readvance(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}