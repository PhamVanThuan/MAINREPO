using Machine.Specifications;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.Activities.Capture_Invoice.OnGetActivityMessage
{
    [Subject("Activity => Capture_Invoice => OnGetActivityMessage")] // AutoGenerated
    internal class when_capture_invoice : WorkflowSpecThirdPartyInvoices
    {
        static string result;
        Establish context = () =>
        {
            result = "abcd";
        };

        Because of = () =>
        {
            result = workflow.GetActivityMessage_Capture_Invoice(instanceData, workflowData, paramsData, messages);
        };

        It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}