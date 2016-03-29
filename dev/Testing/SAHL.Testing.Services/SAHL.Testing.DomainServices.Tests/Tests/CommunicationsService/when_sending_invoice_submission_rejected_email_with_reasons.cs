using NUnit.Framework;
using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
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
    public class when_sending_invoice_submission_rejected_email_with_reasons : ServiceTestBase<ICommunicationsServiceClient>
    {
        [Test]
        [Category("RunInLab")]
        public void when_successful()
        {
            List<IMailAttachment> attachments = new List<IMailAttachment> { new MailAttachment { AttachmentName = "test123.pdf", ContentAsBase64 = "Some Content=", AttachmentType = "pdf" } };
            List<string> rejectionReasons = new List<string> { "Incorrect account number", "No invoice attached" };
            IEmailTemplate<IEmailModel> emailTemaplate = new InvoiceEmailTemplate(InvoiceTemplateType.UnSuccessfulInvoiceEmailTemplate,
                new UnSuccessfulInvoiceSubmissionEmailModel("vishavp@sahomeloans.com", "Account Number", MailPriority.High, rejectionReasons, attachments));
            var command = new SendInternalEmailCommand(Guid.NewGuid(), emailTemaplate);
            base.Execute<SendInternalEmailCommand>(command);
            Assert.IsFalse(messages.HasErrors);
        }
    }
}