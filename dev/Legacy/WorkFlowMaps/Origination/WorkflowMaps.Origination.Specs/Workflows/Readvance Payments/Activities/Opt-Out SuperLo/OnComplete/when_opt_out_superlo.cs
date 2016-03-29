using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Opt_Out_SuperLo.OnComplete
{
    [Subject("Activity => Opt_Out_SuperLo => OnComplete")]
    internal class when_opt_out_superlo : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static IFL furtherLendingClient;

        private Establish context = () =>
        {
            result = false;
            furtherLendingClient = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(furtherLendingClient);
            furtherLendingClient.WhenToldTo(x => x.OptOutSuperLo((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.ADUserName)).Return(true);
        };

        private Because of = () =>
        {
            var message = string.Empty;
            result = workflow.OnCompleteActivity_Opt_Out_SuperLo(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_opt_out_superlo = () =>
        {
            furtherLendingClient.WasToldTo(x => x.OptOutSuperLo((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.ADUserName));
        };

        private It should_return_what_opt_out_superlo_return = () =>
        {
            result.ShouldBeTrue();
        };
    }
}