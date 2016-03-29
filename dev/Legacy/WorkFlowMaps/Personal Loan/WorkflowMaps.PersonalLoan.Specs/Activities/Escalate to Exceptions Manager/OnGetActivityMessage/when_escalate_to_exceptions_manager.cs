using Machine.Specifications;

namespace WorkflowMaps.PersonalLoan.Specs.Activities.Escalate_to_Exceptions_Manager.OnGetActivityMessage
{
    [Subject("Activity => Escalate_to_Exceptions_Manager => OnGetActivityMessage")]
    internal class when_escalate_to_exceptions_manager : WorkflowSpecPersonalLoans
    {
        private static string message;

        private Establish context = () =>
        {
            message = "test";
        };

        private Because of = () =>
        {
            message = workflow.GetActivityMessage_Escalate_to_Exceptions_Manager(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            message.ShouldBeEmpty();
        };
    }
}