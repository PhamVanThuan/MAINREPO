using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Application_in_Order.OnGetStageTransition
{
    [Subject("Activity => Application_in_Order => OnGetStageTransition")]
    internal class when_application_in_order : WorkflowSpecPersonalLoans
    {
        private static string activityMessage;

        private Establish context = () =>
        {
            activityMessage = "Test";
        };

        private Because of = () =>
        {
            activityMessage = workflow.GetStageTransition_Application_in_Order(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            activityMessage.ShouldEqual(string.Empty);
        };
    }
}