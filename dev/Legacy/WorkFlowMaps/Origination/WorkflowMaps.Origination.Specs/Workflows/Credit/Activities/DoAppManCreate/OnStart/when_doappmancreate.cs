using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.DoAppManCreate.OnStart
{
    [Subject("Activity => DoAppManCreate => OnStart")]
    internal class when_doappmancreate : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.EntryPath = 2;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_DoAppManCreate(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}