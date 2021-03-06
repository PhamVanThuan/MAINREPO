using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.Activities.QA_Complete.OnStart
{
    [Subject("Activity => QA_Complete => OnStart")] // AutoGenerated
    internal class when_qa_complete : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_QA_Complete(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}