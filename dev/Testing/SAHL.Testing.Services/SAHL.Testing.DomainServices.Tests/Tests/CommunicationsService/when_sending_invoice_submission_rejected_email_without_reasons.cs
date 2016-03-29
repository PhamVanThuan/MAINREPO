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

namespace SAHL.Testing.Services.Tests.CommunicationsService
{
    [TestFixture]
    public class when_sending_invoice_submission_rejected_email_without_reasons : ServiceTestBase<ICommunicationsServiceClient>
    {
        [Test]
        [Category("RunInLab")]
        public void when_successful()
        {
            List<string> rejectionReseons = new List<string> { };
            IEmailTemplate<IEmailModel> emailTemaplate = new InvoiceEmailTemplate(InvoiceTemplateType.UnSuccessfulInvoiceEmailTemplate,
                new UnSuccessfulInvoiceSubmissionEmailModel("vishavp@sahomeloans.com", "Account Number", MailPriority.High, rejectionReseons, new List<IMailAttachment>()));
            var command = new SendInternalEmailCommand(Guid.NewGuid(), emailTemaplate);
            base.Execute<SendInternalEmailCommand>(command);
            Assert.IsFalse(messages.HasErrors);
        }
    }
}