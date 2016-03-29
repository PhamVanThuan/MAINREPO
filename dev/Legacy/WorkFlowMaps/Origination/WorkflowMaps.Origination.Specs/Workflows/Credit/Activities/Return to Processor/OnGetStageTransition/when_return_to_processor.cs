using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.Return_to_Processor.OnGetStageTransition
{
    [Subject("Activity => Return_To_Processor => OnGetStageTransition")]
    internal class when_return_to_processor : WorkflowSpecCredit
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Return_to_Processor(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_return_to_processor_from_credit = () =>
        {
            result.ShouldBeTheSameAs("Return to Processor from Credit");
        };
    }
}