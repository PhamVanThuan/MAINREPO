using Machine.Specifications;
using WorkflowMaps.Credit.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Credit.Activities.DoAppManCreate.OnStart
{
    [Subject("Activity => DoAppManCreate => OnStart")]
    internal class when_doappmancreate_and_the_entry_path_is_not_2 : WorkflowSpecCredit
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.EntryPath = 1;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_DoAppManCreate(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}