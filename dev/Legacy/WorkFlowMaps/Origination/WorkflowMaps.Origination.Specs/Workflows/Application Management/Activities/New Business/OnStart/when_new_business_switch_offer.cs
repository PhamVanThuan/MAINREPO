using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.New_Business.OnStart
{
    [Subject("Activity => New_Business => OnStart")]
    internal class when_new_business : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.OfferTypeKey = 6; //6 or 7 or 8
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_New_Business(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}