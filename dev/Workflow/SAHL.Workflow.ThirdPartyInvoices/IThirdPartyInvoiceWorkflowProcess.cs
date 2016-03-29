using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Workflow.Maps.Config;

namespace SAHL.Workflow.ThirdPartyInvoices
{
    public interface IThirdPartyInvoiceWorkflowProcess : IWorkflowService
    {
        GetThirdPartyInvoiceQueryResult GetThirdPartyInvoiceByThirdPartyInvoiceKey(ISystemMessageCollection messages, int thirdPartyInvoiceKey);

        bool ApproveThirdPartyInvoice(ISystemMessageCollection messages, int thirdPartyInvoiceKey, IServiceRequestMetadata metadata);

        bool RejectThirdPartyInvoiceByThirdPartyInvoiceKey(ISystemMessageCollection messages, int thirdPartyInvoiceKey, string rejectionComments,
            IServiceRequestMetadata metadata);

        bool ProcessInvoicePayment(ISystemMessageCollection messages, int thirdPartyInvoiceKey, IServiceRequestMetadata metadata);

        bool ArchiveThirdPartyInvoice(ISystemMessageCollection messages, int thirdPartyInvoiceKey, IServiceRequestMetadata metadata);

        bool ReturnInvoiceToPaymentQueue(ISystemMessageCollection messages, int thirdPartyInvoiceKey, IServiceRequestMetadata metadata);

        bool QueryThirdPartyInvoice(ISystemMessageCollection messages, int thirdPartyInvoiceKey, string queryComments, IServiceRequestMetadata metadata);

        bool EscalateThirdPartyInvoiceForApproval(ISystemMessageCollection messages, int thirdPartyInvoiceKey, int uosKeyForEscalatedUser, IServiceRequestMetadata metadata);
    }
}