using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Process_Internet_Lead.OnGetStageTransition
{
    [Subject("Activity => Process_Internet_Lead => OnGetStageTransition")]
    internal class when_process_internet_lead : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Process_Internet_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_process_internet_lead = () =>
        {
            result.ShouldMatch("Process Internet Lead");
        };
    }
}