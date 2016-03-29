using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.EXT_Create_Net_Application.OnComplete
{
    [Subject("Activity => EXT_Create_Net_Application => OnComplete")]
    internal class when_ext_create_net_application : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string message;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.LeadType = 0;
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_EXT_Create_Net_Application(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_lead_type_data_parameter_to_4 = () =>
        {
            workflowData.LeadType.ShouldEqual(4);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}