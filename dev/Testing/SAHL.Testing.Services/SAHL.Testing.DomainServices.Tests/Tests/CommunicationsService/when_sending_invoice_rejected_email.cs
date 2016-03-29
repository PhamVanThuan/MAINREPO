using NUnit.Framework;
using SAHL.Core.Exchange;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Testing.Services.Tests.CommunicationsService
{
    [TestFixture]
    public class when_sending_invoice_rejected_email : ServiceTestBase<ICommunicationsServiceClient>
    {
        [Test]
        public void when_invoice_rejected()
        {
            Random numberGenerator = new Random();
            int automationNUmber = numberGenerator.Next(1000000);
            var @event = new ThirdPartyInvoiceRejectedEvent(DateTime.Now, 1477705, "vishavp@sahomeloans.com", string.Empty, @"SAHL\InvoiceProcessor", "reject test", "SAHL-"+DateTime.Now.ToShortDateString(),
                1293, "1477705 - SAHL-AutomatedTest-"+automationNUmber, Guid.NewGuid());
            var rejectedInvoice = new RejectedThirdPartyInvoiceEmailModel(@event.SAHLReferenceNumber, @event.AccountNumber, @event.ThirdPartyInvoiceKey, @event.AttorneyEmailAddress,
                @event.InvoiceNumber, @event.RejectedBy, @event.RejectionComments, @event.Date, @event.EmailSubject, MailPriority.Normal);
            IEmailTemplate<IEmailModel> emailTemplate = new InvoiceEmailTemplate(InvoiceTemplateType.RejectedInvoiceEmailTemplate, rejectedInvoice);
            var command = new SendInternalEmailCommand(Guid.NewGuid(), emailTemplate);
            base.Execute<SendInternalEmailCommand>(command);
            Assert.IsFalse(messages.HasErrors);
        }
    }
}