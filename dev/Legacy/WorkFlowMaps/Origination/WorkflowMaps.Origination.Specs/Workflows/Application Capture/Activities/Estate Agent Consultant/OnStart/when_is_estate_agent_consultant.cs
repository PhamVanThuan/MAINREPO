using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Estate_Agent_Consultant.OnStart
{
    [Subject("Activity => Estate_Agent_Consultant => OnStart")]
    internal class when_is_estate_agent_consultant : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static IApplicationCapture appCap;

        private Establish context = () =>
        {
            result = false;
            appCap = An<IApplicationCapture>();
            ((InstanceDataStub)instanceData).CreatorADUserName = "ExpectedCreatorADUserName";
            appCap.WhenToldTo(x => x.IsEstateAgent(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Estate_Agent_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_if_creator_aduser_is_estate_agent = () =>
        {
            appCap.WasToldTo(x => x.IsEstateAgent((IDomainMessageCollection)messages, instanceData.CreatorADUserName));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}