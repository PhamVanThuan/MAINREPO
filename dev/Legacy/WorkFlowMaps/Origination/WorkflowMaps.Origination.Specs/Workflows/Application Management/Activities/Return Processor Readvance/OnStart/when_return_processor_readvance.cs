using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Return_Processor_Readvance.OnStart
{
    [Subject("Activity => Return_Processor_Readvance => OnStart")]
    internal class when_return_processor_readvance : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Return_Processor_Readvance(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}