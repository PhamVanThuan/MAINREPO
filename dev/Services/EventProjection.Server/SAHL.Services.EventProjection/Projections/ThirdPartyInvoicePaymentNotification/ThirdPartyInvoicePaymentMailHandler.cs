using SAHL.Core.Events.Projections;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.CATS.Events;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;
using System.Linq;

namespace SAHL.Services.EventProjection.Projections.ThirdPartyInvoicePaymentNotification
{
    public class ThirdPartyInvoicePaymentMailHandler : IServiceProjector<SummarisedPaymentsToRecipientEvent, ICommunicationsServiceClient>
    {
        private ICombGuid combGuidGenerator;

        public ThirdPartyInvoicePaymentMailHandler(ICombGuid combGuidGenerator)
        {
            this.combGuidGenerator = combGuidGenerator;
        }

        public void Handle(SummarisedPaymentsToRecipientEvent @event, IServiceRequestMetadata metadata, ICommunicationsServiceClient communicationsServiceClient)
        {
            var invoicePayments = @event.InvoicePayments.Select(x =>
                new InvoicePaymentModel(x.AccountKey, x.SahlReferenceNumber, x.Amount)).ToList();

            var invoiceScheduledForPaymentModel = new InvoiceScheduledForPaymentEmailModel(
                  @event.InvoicePayments.First().TargetName
                , @event.EmailAddress
                , System.Net.Mail.MailPriority.High
                , invoicePayments);

            var emailTemplate = new InvoiceEmailTemplate(InvoiceTemplateType.InvoiceScheduledForPaymentEmailTemplate, invoiceScheduledForPaymentModel);

            var sendThirdPartyInvoicePaymentMailCommand = new SendInternalEmailCommand(combGuidGenerator.Generate(), emailTemplate);

            communicationsServiceClient.PerformCommand(sendThirdPartyInvoicePaymentMailCommand, metadata);
        }
    }
}