using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Is_Estate_Agent_Lead.OnStart
{
    [Subject("Activity => Is_Estate_Agent_Lead => OnStart")]
    internal class when_is_estate_agent_lead_where_lead_type_is_0_but_is_not_estate_agent : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static IWorkflowAssignment assignment;
        private static IApplicationCapture appCap;

        private Establish context = () =>
        {
            result = true;
            workflowData.LeadType = 0;
            ((InstanceDataStub)instanceData).CreatorADUserName = "ExpectedCreatorADUserName";
            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            appCap = An<IApplicationCapture>();
            appCap.WhenToldTo(x => x.IsEstateAgent(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Is_Estate_Agent_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_if_is_estate_agent = () =>
        {
            appCap.WasToldTo(x => x.IsEstateAgent((IDomainMessageCollection)messages, instanceData.CreatorADUserName));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}