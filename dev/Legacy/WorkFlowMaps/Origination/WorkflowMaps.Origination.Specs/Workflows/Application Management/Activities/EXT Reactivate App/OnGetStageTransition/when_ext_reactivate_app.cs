using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.EXT_Reactivate_App.OnGetStageTransition
{
    [Subject("Activity => EXT_Reactivate_App => OnGetStageTransition")] // AutoGenerated
    internal class when_ext_reactivate_app : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_EXT_Reactivate_App(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}