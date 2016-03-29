using SAHL.Core.Events.Projections;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.ThirdPartyInvoiceRejectionNotification
{
    public class ThirdPartyInvoiceRejectionMailHandler : IServiceProjector<ThirdPartyInvoiceRejectedPreApprovalEvent, ICommunicationsServiceClient>
        , IServiceProjector<ThirdPartyInvoiceRejectedPostApprovalEvent, ICommunicationsServiceClient>
    {
        private ICombGuid combGuidGenerator;

        public ThirdPartyInvoiceRejectionMailHandler(ICombGuid combGuidGenerator)
        {
            this.combGuidGenerator = combGuidGenerator;
        }

        public void Handle(ThirdPartyInvoiceRejectedPostApprovalEvent @event, IServiceRequestMetadata metadata, ICommunicationsServiceClient service)
        {
            SendRejectionEmail(@event, metadata, service);
        }

        public void Handle(ThirdPartyInvoiceRejectedPreApprovalEvent @event, IServiceRequestMetadata metadata, ICommunicationsServiceClient service)
        {
            SendRejectionEmail(@event, metadata, service);
        }

        private void SendRejectionEmail(ThirdPartyInvoiceRejectedEvent @event, IServiceRequestMetadata metadata, ICommunicationsServiceClient service)
        {
            var rejectedInvoiceMailModel = new RejectedThirdPartyInvoiceEmailModel(
                              @event.SAHLReferenceNumber
                            , @event.AccountNumber
                            , @event.ThirdPartyInvoiceKey
                            , @event.AttorneyEmailAddress
                            , @event.InvoiceNumber
                            , @event.RejectedBy
                            , @event.RejectionComments
                            , @event.Date
                            , @event.EmailSubject
                            , System.Net.Mail.MailPriority.Normal
                        );

            var emailTemplate = new InvoiceEmailTemplate(Interfaces.Communications.Enums.InvoiceTemplateType.RejectedInvoiceEmailTemplate, rejectedInvoiceMailModel);

            var sendThirdPartyInvoiceRejectedMailCommand = new SendInternalEmailCommand(combGuidGenerator.Generate(), emailTemplate);

            service.PerformCommand(sendThirdPartyInvoiceRejectedMailCommand, metadata);
        }
    }
}