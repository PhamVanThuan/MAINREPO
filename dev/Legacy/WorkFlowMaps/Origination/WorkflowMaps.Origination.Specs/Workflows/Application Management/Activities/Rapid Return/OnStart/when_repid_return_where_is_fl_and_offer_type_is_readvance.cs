using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Rapid_Return.OnStart
{
    [Subject("Activity => Rapid_Return => OnStart")]
    internal class when_repid_return_where_is_fl_and_offer_type_is_readvance : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsFL = true;
            workflowData.OfferTypeKey = (int)SAHL.Common.Globals.OfferTypes.ReAdvance;
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Rapid_Return(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}