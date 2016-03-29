using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Further_Lending.OnStart
{
    [Subject("Activity => Further_Lending => OnStart")]
    internal class when_further_lending_and_is_not_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.IsFL = false;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Further_Lending(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldEqual(false);
        };
    }
}