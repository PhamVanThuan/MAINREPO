using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using System;

namespace SAHL.DomainProcessManager.DomainProcesses.Managers.X2Workflow
{
    public interface IX2WorkflowManager
    {
        ISystemMessageCollection CreateWorkflowCase(int applicationNumber, DomainProcessServiceRequestMetadata domainServiceMetadata);

        ISystemMessageCollection ProcessThirdPartyInvoicePayment(long instanceId, int accountKey, int thirdPartyInvoiceKey, DomainProcessServiceRequestMetadata domainServiceMetadata);

        ISystemMessageCollection ArchiveThirdPartyInvoice(long instanceId, int accountKey, int thirdPartyInvoiceKey, DomainProcessServiceRequestMetadata domainServiceMetadata);

        ISystemMessageCollection ReversePayment(long instanceId, int accountKey, int thirdPartyInvoiceKey, DomainProcessServiceRequestMetadata domainServiceMetadata);
    }
}