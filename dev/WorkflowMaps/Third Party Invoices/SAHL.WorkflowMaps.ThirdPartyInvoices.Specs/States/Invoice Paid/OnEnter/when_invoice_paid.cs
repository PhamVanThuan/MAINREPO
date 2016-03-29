using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Workflow.ThirdPartyInvoices;

namespace SAHL.WorkflowMaps.ThirdPartyInvoices.Specs.States.Invoice_Paid.OnEnter
{
    [Subject("State => Invoice_Paid => OnEnter")] // AutoGenerated
    internal class when_invoice_paid : WorkflowSpecThirdPartyInvoices
    {
        private static bool result;
        private static IThirdPartyInvoiceWorkflowProcess domainProcess;

        private Establish context = () =>
        {
            domainProcess = An<IThirdPartyInvoiceWorkflowProcess>();
            domainProcess.WhenToldTo(x => x.ArchiveThirdPartyInvoice(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()))
            .Return(true);
            domainServiceLoader.RegisterMockForType(domainProcess);
            result = false;
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Invoice_Paid(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_the_invoice_using_the_domain_process = () =>
        {
            domainProcess.WasToldTo(x => x.ArchiveThirdPartyInvoice(Param.IsAny<ISystemMessageCollection>(), workflowData.ThirdPartyInvoiceKey, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}