using Machine.Specifications;

namespace WorkflowMaps.ApplicationManagement.Specs.States.Archive_QA_Query.OnReturn
{
    [Subject("State => Archive_QA_Query => OnReturn")] // AutoGenerated
    internal class when_archive_qa_query : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnReturn_Archive_QA_Query(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}