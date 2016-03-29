using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.EXT_NTU.OnGetStageTransition
{
    [Subject("Activity => EXT_NTU => OnGetStageTransition")] // AutoGenerated
    internal class when_ext_ntu : WorkflowSpecApplicationManagement
    {
        private static string result;

        private Establish context = () =>
        {
            result = "abcd";
        };

        private Because of = () =>
        {
            result = workflow.GetStageTransition_EXT_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_an_empty_string = () =>
        {
            result.ShouldBeTheSameAs(string.Empty);
        };
    }
}