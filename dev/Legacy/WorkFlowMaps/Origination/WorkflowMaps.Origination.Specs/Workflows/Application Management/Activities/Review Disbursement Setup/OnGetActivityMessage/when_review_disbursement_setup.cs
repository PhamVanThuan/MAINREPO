using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Review_Disbursement_Setup.OnGetActivityMessage
{
    [Subject("Activity => Review_Disbursement_Setup => OnGetActivityMessage")] // AutoGenerated
    internal class when_review_disbursement_setup : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Review_Disbursement_Setup(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}