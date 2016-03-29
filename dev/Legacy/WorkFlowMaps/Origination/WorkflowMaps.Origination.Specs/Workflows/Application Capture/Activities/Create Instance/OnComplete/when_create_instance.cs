using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Create_Instance.OnComplete
{
    [Subject("Activity => Create_Instance => OnComplete")]
    internal class when_create_instance : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string message;
        private static int expectedLeadType;

        private Establish context = () =>
        {
            workflowData.LeadType = 0;
            expectedLeadType = 1;//Create Clone
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Create_Instance(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_lead_type_data_property_to_create_clone = () =>
        {
            workflowData.LeadType.ShouldEqual<int>(expectedLeadType);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}