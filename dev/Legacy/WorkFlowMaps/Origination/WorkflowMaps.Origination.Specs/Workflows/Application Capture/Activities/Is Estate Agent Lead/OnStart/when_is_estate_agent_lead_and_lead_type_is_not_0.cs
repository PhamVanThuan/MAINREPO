using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Is_Estate_Agent_Lead.OnStart
{
    [Subject("Activity => Is_Estate_Agent_Lead => OnStart")]
    internal class when_is_estate_agent_lead_and_lead_type_is_not_0 : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static IWorkflowAssignment assignment;
        private static IApplicationCapture appCap;

        private Establish context = () =>
        {
            result = true;
            workflowData.LeadType = 1;
            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            appCap = An<IApplicationCapture>();
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Is_Estate_Agent_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_check_if_is_estate_agent = () =>
        {
            appCap.WasNotToldTo(x => x.IsEstateAgent(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>()));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}