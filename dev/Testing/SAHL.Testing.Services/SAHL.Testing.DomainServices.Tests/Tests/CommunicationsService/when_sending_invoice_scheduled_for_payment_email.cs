using NUnit.Framework;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace SAHL.Testing.Services.Tests.CommunicationsService
{
    [TestFixture]
    public class when_sending_invoice_scheduled_for_payment_email : ServiceTestBase<ICommunicationsServiceClient>
    {
        [Test]
        public void when_successful()
        {
            List<InvoicePaymentModel> invoicePayments = new List<InvoicePaymentModel>();
            invoicePayments.Add(new InvoicePaymentModel(1408282, "SAHL-111-222-333", 2000));
            invoicePayments.Add(new InvoicePaymentModel(1428540, "SAHL-111-222-33", 1111));
            invoicePayments.Add(new InvoicePaymentModel(1428540, "SAHL-111-222-333", 2222));
            invoicePayments.Add(new InvoicePaymentModel(1428540, "SAHL-111-222-333", 3333));
            invoicePayments.Add(new InvoicePaymentModel(1428540, "SAHL-111-222-333", 3000.58M));
            invoicePayments.Add(new InvoicePaymentModel(1428540, "SAHL-111-222-333", 3000));
            invoicePayments.Add(new InvoicePaymentModel(1428540, "SAHL-111-222-333", 9999));
            invoicePayments.Add(new InvoicePaymentModel(1428540, "SAHL-111-222-333", 3000));
            invoicePayments.Add(new InvoicePaymentModel(1428540, "SAHL-111-222-333", 3000));
            invoicePayments.Add(new InvoicePaymentModel(1428540, "SAHL-111-222-333", 3000));
            invoicePayments.Add(new InvoicePaymentModel(1428540, "SAHL-111-222-333", 3000));
            var invoiceScheduledForPaymentEmailModel = new InvoiceScheduledForPaymentEmailModel("Clint Speed", "clintons@sahomeloans.com", MailPriority.Normal, invoicePayments);
            IEmailTemplate<IEmailModel> emailTemplate = new InvoiceEmailTemplate(InvoiceTemplateType.InvoiceScheduledForPaymentEmailTemplate, invoiceScheduledForPaymentEmailModel);
            var command = new SendInternalEmailCommand(Guid.NewGuid(), emailTemplate);
            base.Execute<SendInternalEmailCommand>(command).WithoutErrors();
        }
    }
}