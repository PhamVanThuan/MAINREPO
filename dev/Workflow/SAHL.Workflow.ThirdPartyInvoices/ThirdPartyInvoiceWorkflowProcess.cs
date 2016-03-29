using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FinanceDomain.Queries;
using System.Linq;

namespace SAHL.Workflow.ThirdPartyInvoices
{
    public class ThirdPartyInvoiceWorkflowProcess : IThirdPartyInvoiceWorkflowProcess
    {
        private IFinanceDomainServiceClient financeDomainServiceClient;

        public ThirdPartyInvoiceWorkflowProcess(IFinanceDomainServiceClient financeDomainServiceClient)
        {
            this.financeDomainServiceClient = financeDomainServiceClient;
        }

        public GetThirdPartyInvoiceQueryResult GetThirdPartyInvoiceByThirdPartyInvoiceKey(ISystemMessageCollection messages, int thirdPartyInvoiceKey)
        {
            var getThirdPartyInvoiceQuery = new GetThirdPartyInvoiceQuery(thirdPartyInvoiceKey);
            financeDomainServiceClient.PerformQuery(getThirdPartyInvoiceQuery);
            return getThirdPartyInvoiceQuery.Result.Results.FirstOrDefault();
        }

        public bool ApproveThirdPartyInvoice(ISystemMessageCollection messages, int thirdPartyInvoiceKey, IServiceRequestMetadata metadata)
        {
            var approveThirdPartyInvoiceCommand = new ApproveThirdPartyInvoiceCommand(thirdPartyInvoiceKey);
            messages.Aggregate(financeDomainServiceClient.PerformCommand(approveThirdPartyInvoiceCommand, metadata));

            return !messages.HasErrors;
        }

        public bool RejectThirdPartyInvoiceByThirdPartyInvoiceKey(ISystemMessageCollection messages, int thirdPartyInvoiceKey, string rejectionComments,
            IServiceRequestMetadata metadata)
        {
            var command = new RejectThirdPartyInvoiceCommand(thirdPartyInvoiceKey, rejectionComments);
            var commandMessage = financeDomainServiceClient.PerformCommand(command, metadata);

            messages.Aggregate(commandMessage);
            return !messages.HasErrors;
        }

        public bool QueryThirdPartyInvoice(ISystemMessageCollection messages, int thirdPartyInvoiceKey, string queryComments, IServiceRequestMetadata metadata)
        {
            var command = new QueryThirdPartyInvoiceCommand(thirdPartyInvoiceKey, queryComments);
            messages.Aggregate(financeDomainServiceClient.PerformCommand(command, metadata));
            return !messages.HasErrors;
        }

        public bool ArchiveThirdPartyInvoice(ISystemMessageCollection messages, int thirdPartyInvoiceKey, IServiceRequestMetadata metadata)
        {
            var command = new MarkThirdPartyInvoiceAsPaidCommand(thirdPartyInvoiceKey);
            messages.Aggregate(financeDomainServiceClient.PerformCommand(command, metadata));
            return !messages.HasErrors;
        }

        public bool ReturnInvoiceToPaymentQueue(ISystemMessageCollection messages, int thirdPartyInvoiceKey, IServiceRequestMetadata metadata)
        {
            var command = new ReturnThirdPartyInvoiceToPaymentQueueCommand(thirdPartyInvoiceKey);
            messages.Aggregate(financeDomainServiceClient.PerformCommand(command, metadata));
            return !messages.HasErrors;
        }

        public bool ProcessInvoicePayment(ISystemMessageCollection messages, int thirdPartyInvoiceKey, IServiceRequestMetadata metadata)
        {
            var command = new ProcessThirdPartyInvoicePaymentCommand(thirdPartyInvoiceKey);
            messages.Aggregate(financeDomainServiceClient.PerformCommand(command, metadata));
            return !messages.HasErrors;
        }

        public bool EscalateThirdPartyInvoiceForApproval(ISystemMessageCollection messages, int thirdPartyInvoiceKey, int uosKeyForEscalatedUser, IServiceRequestMetadata metadata)
        {
            var command = new EscalateThirdPartyInvoiceForApprovalCommand(thirdPartyInvoiceKey, uosKeyForEscalatedUser);
            messages.Aggregate(financeDomainServiceClient.PerformCommand(command, metadata));
            return !messages.HasErrors;
        }
    }
}