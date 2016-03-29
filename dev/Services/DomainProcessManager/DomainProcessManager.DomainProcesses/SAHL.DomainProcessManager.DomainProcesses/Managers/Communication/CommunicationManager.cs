using SAHL.Core.Exchange;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SAHL.DomainProcessManager.DomainProcesses.Managers.Communication
{
    public class CommunicationManager : ICommunicationManager
    {
        private ICommunicationsServiceClient communicationsService;
        private ICombGuid combGuidGenerator;
        private ICommunicationManagerSettings settings;

        public CommunicationManager(ICommunicationsServiceClient communicationsService, ICombGuid combGuidGenerator, ICommunicationManagerSettings settings)
        {
            this.communicationsService = communicationsService;
            this.combGuidGenerator = combGuidGenerator;
            this.settings = settings;
        }

        public void SendClientDetailComparisonFailedEmail(Dictionary<string, string> differences, string legalEntityName, string identityNumber, int applicationNumber, bool dateOfBirthSetToComcorpProvided)
        {
            string subject = string.Format("Application {0}", applicationNumber);

            Dictionary<string, string> differingRecords = new Dictionary<string, string>();
            foreach (var difference in differences)
            {
                var key = Regex.Replace(difference.Key, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                differingRecords.Add(key, difference.Value);
            }

            var emailModel = new ComcorpComparisonEmailModel(settings.CamTeamEmailAddress, subject, differingRecords, MailPriority.Normal, identityNumber, legalEntityName, dateOfBirthSetToComcorpProvided);
            IEmailTemplate<IEmailModel> camTeamEmail = new ComcorpComparisonEmailTemplate(emailModel);

            SendInternalMail(camTeamEmail);
        }

        public void SendNonCriticalErrorsEmail(ISystemMessageCollection systemMessageCollection, int applicationNumber)
        {
            string subject = string.Format("Errors requiring your attention on Comcorp application number {0}", applicationNumber);
            string messageBody = "Please attend to the following issues that were encountered while processing the application: \n";

            int messageNumber = 0;
            foreach (var message in systemMessageCollection.ErrorMessages().Where(m => m.Severity != SystemMessageSeverityEnum.Exception))
            {
                messageNumber += 1;
                messageBody += string.Format("\n {0}. {1}", messageNumber, message.Message);
            }

            var emailModel = new StandardEmailModel(settings.CamTeamEmailAddress, subject, messageBody, MailPriority.Normal);
            IEmailTemplate<IEmailModel> camTeamApplicationIssuesEmail = new StandardEmailTemplate(emailModel);

            SendInternalMail(camTeamApplicationIssuesEmail);
        }

        public void SendX2CaseCreationFailedSupportEmail(ISystemMessageCollection systemMessageCollection, Guid domainProcessId, int applicationNumber)
        {
            string subject = string.Format("X2 Case Creation failure for Comcorp application number {0}", applicationNumber);
            string messageBody = "The following exceptions were encountered while processing the application: \n";

            int messageNumber = 0;
            foreach (var message in systemMessageCollection.ErrorMessages().Where(m => m.Severity == SystemMessageSeverityEnum.Exception))
            {
                messageNumber += 1;
                messageBody += string.Format("\n {0}. {1}", messageNumber, message.Message);
            }

            var emailModel = new StandardEmailModel(settings.ITFrontEndTeamEmailAddress, subject, messageBody, MailPriority.Normal);
            IEmailTemplate<IEmailModel> itFrontEndTeamEmail = new StandardEmailTemplate(emailModel);

            SendInternalMail(itFrontEndTeamEmail);
        }

        private void SendInternalMail(IEmailTemplate<IEmailModel> specificEmail)
        {
            var sendEmailCorrelationId = combGuidGenerator.Generate();
            IServiceRequestMetadata serviceRequestMetadata = new ServiceRequestMetadata();
            var sendEmailCommand = new SendInternalEmailCommand(sendEmailCorrelationId, specificEmail);
            this.communicationsService.PerformCommand(sendEmailCommand, serviceRequestMetadata);
        }

        public void SendAcceptInvoiceErrorEmail(ISystemMessageCollection systemMessageCollection, Guid domainProcessId, int attorneyInvoiceKey)
        {
            var body = string.Join(" ", systemMessageCollection.AllMessages.Select(x => x.Message).ToArray());
            var model = new StandardEmailModel(settings.ITFrontEndTeamEmailAddress, "An error occured while accepting an invoice.", body, MailPriority.High, null);
            var emailTemplate = new StandardEmailTemplate(model);
            SendInternalMail(emailTemplate);
        }

        public void SendAcceptInvoiceConfirmationEmail(InvoiceTemplateType templateName, Guid domainProcessId, IEmailModel model)
        {
            var emailTemplate = new InvoiceEmailTemplate(templateName, model);
            var command = new SendInternalEmailCommand(combGuidGenerator.Generate(), emailTemplate);
            communicationsService.PerformCommand(command, new ServiceRequestMetadata());
        }

        public void SendProcessingFailedEmailToLossControl(string emailSubject, List<IMailAttachment> attachments, ISystemMessageCollection systemMessageCollection)
        {
            var emailBody = @"The attorney invoice attached could not be processed due to an internal system error.
                             You are required to liase with IT prior to resending the email back to the system mailbox.";

            var emailModel = new StandardEmailModel(settings.AttorneyInvoiceFailuresEmailAddress, "Failed: " + emailSubject, emailBody, MailPriority.High, attachments);
            var emailTemplate = new StandardEmailTemplate(emailModel);
            var command = new SendInternalEmailCommand(combGuidGenerator.Generate(), emailTemplate);

            communicationsService.PerformCommand(command, new ServiceRequestMetadata());
        }

        public void SendOperatorRequestForManualArchive(List<string> stuckThirdPartyInvoiceSAHLReferences)
        {
            var emailBody = @"The following attorney invoices could not be archived due to an internal system error.
                             Please manually move the cases into the invoice paid archive. \n\n";
            emailBody = string.Concat(emailBody, stuckThirdPartyInvoiceSAHLReferences.Aggregate((i, j) => i + ", " + j));

            var emailModel = new StandardEmailModel(settings.ThirdPartyInvoiceProcessorEmailAddress, "Notifying Beneficiaries Failed", emailBody, MailPriority.High, null);
            var emailTemplate = new StandardEmailTemplate(emailModel);
            var command = new SendInternalEmailCommand(combGuidGenerator.Generate(), emailTemplate);

            communicationsService.PerformCommand(command, new ServiceRequestMetadata());
        }

        public void SendFailureToNotifyRecipientsEmail(string[] SAHLReferencesForFailedRecipientNotifications)
        {
            var emailBody = @"The system failed to notify payment recipients for the following attorney invoices. \n\n";
            emailBody = string.Concat(emailBody, SAHLReferencesForFailedRecipientNotifications.Aggregate((i, j) => i + ", " + j));

            var emailModel = new StandardEmailModel(settings.ThirdPartyInvoiceProcessorEmailAddress, "Notifying Payment Recipients Failed", emailBody, MailPriority.High, null);
            var emailTemplate = new StandardEmailTemplate(emailModel);
            var command = new SendInternalEmailCommand(combGuidGenerator.Generate(), emailTemplate);

            communicationsService.PerformCommand(command, new ServiceRequestMetadata());
        }
    }
}