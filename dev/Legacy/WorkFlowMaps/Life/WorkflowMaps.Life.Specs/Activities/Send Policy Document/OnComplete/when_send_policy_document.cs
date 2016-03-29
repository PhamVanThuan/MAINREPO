using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.Life;

namespace WorkflowMaps.Life.Specs.Activities.Send_Policy_Document.OnComplete
{
    [Subject("Activity => Send_Policy_Document => OnComplete")]
    internal class when_send_policy_document : WorkflowSpecLife
    {
        private static ILife client;
        private static string message;

        private Establish context = () =>
        {
            client = An<ILife>();
            domainServiceLoader.RegisterMockForType<ILife>(client);
            workflowData.OfferKey = 1;
        };

        private Because of = () =>
        {
            workflow.OnCompleteActivity_Send_Policy_Document(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_activate_the_life_policy = () =>
        {
            client.WasToldTo(x => x.ActivateLifePolicy((IDomainMessageCollection)messages, workflowData.OfferKey));
        };
    }
}