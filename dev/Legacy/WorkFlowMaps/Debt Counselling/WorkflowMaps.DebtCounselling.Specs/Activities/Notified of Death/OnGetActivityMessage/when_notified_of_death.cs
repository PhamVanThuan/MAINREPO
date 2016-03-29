using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Notified_of_Death.OnGetActivityMessage
{
    [Subject("Activity => Notified_of_Death => OnGetActivityMessage")] // AutoGenerated
    internal class when_notified_of_death : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetActivityMessage_Notified_of_Death(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}