using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Rapid_Readvance.OnStart
{
    [Subject("Activity => Rapid_Readvance => OnStart")]
    internal class when_rapid_readvance : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IFL furtherLendingClient;

        private Establish context = () =>
        {
            furtherLendingClient = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(furtherLendingClient);

            furtherLendingClient.WhenToldTo(x => x.IsReadvanceAdvanceApplication((IDomainMessageCollection)messages, workflowData.ApplicationKey)).Return(true);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Rapid_Readvance(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_if_readvance_application = () =>
        {
            furtherLendingClient.WasToldTo(x => x.IsReadvanceAdvanceApplication((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}