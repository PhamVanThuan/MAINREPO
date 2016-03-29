using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.New_Business.OnStart
{
    [Subject("Activity => New_Business => OnStart")]
    internal class when_further_lending : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.OfferTypeKey = 2; //2 or 3 ro 4
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_New_Business(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}