using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Reallocate_User.OnGetActivityMessage
{
    [Subject("Activity => Reallocate_User => OnGetActivityMessage")] // AutoGenerated
    internal class when_reallocate_user : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Reallocate_User(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}