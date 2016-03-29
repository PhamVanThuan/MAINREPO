using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Rapid_Return.OnStart
{
    [Subject("Activity => Rapid_Return => OnStart")]
    internal class when_rapid_return_and_is_not_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.IsFL = false;
            workflowData.OfferTypeKey = (int)SAHL.Common.Globals.OfferTypes.ReAdvance;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Rapid_Return(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}