using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.Valuations.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Valuations.Activities.Request_Lightstone_Valuation.OnComplete
{
    [Subject("Activity => Request_Lightstone_Valuation => OnComplete")]
    internal class when_request_lightstone_valuation_rules_fail : WorkflowSpecValuations
    {
        private static bool result;
        private static ICommon client;
        private static string message;

        private Establish context = () =>
        {
            client = An<ICommon>();
            result = true;
            domainServiceLoader.RegisterMockForType<ICommon>(client);
            client.WhenToldTo(x => x.CheckPropertyExists((IDomainMessageCollection)messages, workflowData.ApplicationKey)).Return(true);
            client.WhenToldTo(x => x.CheckLightStoneValuationRules((IDomainMessageCollection)messages,
                        workflowData.ApplicationKey, paramsData.IgnoreWarning)).Return(false);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Request_Lightstone_Valuation(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_check_lightstone_valuation_rules = () =>
        {
            client.WasToldTo(x => x.CheckLightStoneValuationRules((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.IgnoreWarning));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}