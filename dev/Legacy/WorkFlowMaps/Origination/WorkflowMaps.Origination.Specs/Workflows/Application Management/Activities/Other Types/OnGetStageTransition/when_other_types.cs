using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.Other_Types.OnGetStageTransition
{
    [Subject("Activity => Other_Types => OnGetStageTransition")] // AutoGenerated
    internal class when_other_types : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_Other_Types(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}