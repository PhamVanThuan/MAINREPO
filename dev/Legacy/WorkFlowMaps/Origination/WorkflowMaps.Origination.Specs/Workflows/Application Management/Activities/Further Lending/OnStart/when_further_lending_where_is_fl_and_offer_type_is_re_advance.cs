using Machine.Specifications;
using WorkflowMaps.ApplicationManagement.Specs;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Further_Lending.OnStart
{
    [Subject("Activity => Further_Lending => OnStart")]
    internal class when_further_lending_where_is_fl_and_offer_type_is_further_lending : WorkflowSpecApplicationManagement
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsFL = true;
            workflowData.OfferTypeKey = 2; //or 3 or 4
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