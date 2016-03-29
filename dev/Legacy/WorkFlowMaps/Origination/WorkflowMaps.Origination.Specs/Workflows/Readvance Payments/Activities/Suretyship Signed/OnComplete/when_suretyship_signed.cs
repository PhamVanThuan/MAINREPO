using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ReadvancePayments.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Suretyship_Signed.OnComplete
{
    [Subject("State => Suretyship_Signed => OnComplete")]
    internal class when_suretyship_signed : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static string message;
        private static IFL fl;

        private Establish context = () =>
        {
            result = true;
            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Suretyship_Signed(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_confirm_suretyship_signed = () =>
        {
            fl.WasToldTo(x => x.SuretySignedConfirmed((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}