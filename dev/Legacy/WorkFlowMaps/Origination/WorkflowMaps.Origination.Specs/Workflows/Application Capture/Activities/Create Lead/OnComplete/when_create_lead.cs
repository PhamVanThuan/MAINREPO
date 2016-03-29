using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Create_Lead.OnComplete
{
    [Subject("Activity => Create_Lead => OnComplete")]
    internal class when_create_lead : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static int expectedLeadType;
        private static string message;

        private Establish context = () =>
        {
            workflowData.LeadType = 200;
            expectedLeadType = 0;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_Lead(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_return_true = () =>
        {
            workflowData.LeadType.ShouldEqual<int>(expectedLeadType);
        };
    }
}