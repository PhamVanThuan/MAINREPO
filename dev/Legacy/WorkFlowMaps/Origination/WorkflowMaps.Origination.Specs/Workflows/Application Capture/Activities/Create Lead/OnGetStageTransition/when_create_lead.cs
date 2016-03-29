using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Create_Lead.OnGetStageTransition
{
    [Subject("Activity => Create_Lead => OnGetStageTransition")]
    internal class when_create_lead : WorkflowSpecApplicationCapture
    {
        private static string result;

        private Establish context = () =>
        {
            result = string.Empty;
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Create_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_string_create_lead = () =>
        {
            result.ShouldEqual("Create Lead");
        };
    }
}