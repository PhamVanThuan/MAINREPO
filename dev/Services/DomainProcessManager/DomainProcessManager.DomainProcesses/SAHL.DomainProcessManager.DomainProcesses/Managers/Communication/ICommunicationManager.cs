using SAHL.Core.Exchange;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Managers.Communication
{
    public interface ICommunicationManager
    {
        void SendClientDetailComparisonFailedEmail(Dictionary<string, string> differences, string legalEntityName, string identityNumber, int applicationNumber, bool dateOfBirthSetToComcorpProvided);

        void SendNonCriticalErrorsEmail(ISystemMessageCollection systemMessageCollection, int applicationNumber);

        void SendX2CaseCreationFailedSupportEmail(ISystemMessageCollection systemMessageCollection, Guid domainProcessId, int applicationNumber);

        void SendAcceptInvoiceErrorEmail(ISystemMessageCollection systemMessageCollection, Guid domainProcessId, int attorneyInvoiceKey);

        void SendAcceptInvoiceConfirmationEmail(InvoiceTemplateType templateName, Guid domainProcessId, IEmailModel model);

        void SendProcessingFailedEmailToLossControl(string emailSubject, List<IMailAttachment> attachments, ISystemMessageCollection systemMessageCollection);

        void SendOperatorRequestForManualArchive(List<string> stuckThirdPartyInvoiceSAHLReferences);

        void SendFailureToNotifyRecipientsEmail(string[] SAHLReferencesForFailedReceipientNotifications);
    }
}