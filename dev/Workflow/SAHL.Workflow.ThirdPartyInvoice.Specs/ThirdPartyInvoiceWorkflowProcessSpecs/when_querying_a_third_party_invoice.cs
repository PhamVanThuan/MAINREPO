using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Workflow.ThirdPartyInvoices;

namespace SAHL.Workflow.ThirdPartyInvoice.Specs.ThirdPartyInvoiceWorkflowProcessSpecs
{
    public class when_querying_a_third_party_invoice : WithFakes
    {
        private static IThirdPartyInvoiceWorkflowProcess workflowProcess;
        private static IFinanceDomainServiceClient financeDomainServiceClient;
        private static IServiceRequestMetadata metadata;
        private static ISystemMessageCollection messages;
        private static int thirdPartyInvoiceKey;
        private static bool result;
        private static string queryComments;

        private Establish context = () =>
        {
            queryComments = "Horrible Invoice!";
            financeDomainServiceClient = An<IFinanceDomainServiceClient>();
            workflowProcess = new ThirdPartyInvoiceWorkflowProcess(financeDomainServiceClient);
            metadata = An<IServiceRequestMetadata>();

            thirdPartyInvoiceKey = 232;
            messages = SystemMessageCollection.Empty();
        };

        private Because of = () =>
        {
            result = workflowProcess.QueryThirdPartyInvoice(messages, thirdPartyInvoiceKey, queryComments, metadata);
        };

        private It should_query_the_third_party_invoice = () =>
        {
            financeDomainServiceClient.WasToldTo(x => x.PerformCommand(Arg.Is<QueryThirdPartyInvoiceCommand>
                (y=>y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey), metadata));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}