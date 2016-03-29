using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Lead___OfferType_NONE.OnStart
{
    [Subject("Activity => Lead___OfferType_NONE => OnStart")]
    internal class when_lead_type_data_property_is_not_0 : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.LeadType = 1;
            var assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Lead___OfferType_NONE(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}