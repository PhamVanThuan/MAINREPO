using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Misc_Condition.OnGetStageTransition
{
    [Subject("Activity => Misc_Condition => OnGetStageTransition")] // AutoGenerated
    internal class when_misc_condition : WorkflowSpecApplicationManagement
    {
        static string result;
        Establish context = () =>
        {
            result = "abcd";
        };

        Because of = () =>
        {
            result = workflow.GetStageTransition_Misc_Condition(instanceData, workflowData, paramsData, messages);
        };

        It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}