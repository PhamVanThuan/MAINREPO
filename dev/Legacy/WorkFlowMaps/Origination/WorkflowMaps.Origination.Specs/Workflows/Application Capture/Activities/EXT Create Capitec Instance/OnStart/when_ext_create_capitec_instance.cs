using Machine.Specifications;

namespace WorkflowMaps.ApplicationCapture.Specs.Activities.EXT_Create_Capitec_Instance.OnStart
{
    [Subject("Activity => EXT_Create_Capitec_Instance => OnStart")] // AutoGenerated
    internal class when_ext_create_capitec_instance : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_EXT_Create_Capitec_Instance(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}