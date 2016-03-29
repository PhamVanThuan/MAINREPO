using Machine.Specifications;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Not_Direct_Consultant.OnStart
{
    [Subject("Activity => Not_Direct_Consultant => OnStart")]
    internal class when_lead_type_is_not_1 : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            workflowData.LeadType = 2;

            var client = An<IApplicationCapture>();
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(client);

            var assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Not_Direct_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}