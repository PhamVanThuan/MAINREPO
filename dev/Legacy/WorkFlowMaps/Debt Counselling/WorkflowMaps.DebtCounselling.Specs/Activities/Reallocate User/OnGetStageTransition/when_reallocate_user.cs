using Machine.Specifications;

namespace WorkflowMaps.DebtCounselling.Specs.Activities.Reallocate_User.OnGetStageTransition
{
    [Subject("Activity => Reallocate_User => OnGetStageTransition")] // AutoGenerated
    internal class when_reallocate_user : WorkflowSpecDebtCounselling
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Reallocate_User(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}