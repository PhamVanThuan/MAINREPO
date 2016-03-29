using NUnit.Framework;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;
using System;
using System.Net.Mail;

namespace SAHL.Testing.Services.Tests.CommunicationsService
{
    [TestFixture]
    public class when_sending_invoice_received_successfull_email : ServiceTestBase<ICommunicationsServiceClient>
    {
        [Test]
        [Category("RunInLab")]
        public void when_successful()
        {
            IEmailTemplate<IEmailModel> emailTemplate = new InvoiceEmailTemplate(InvoiceTemplateType.SuccessfulInvoiceEmailTemplate,
                new SuccessfulInvoiceSubmissionEmailModel("takawirat@sahomeloans.com", "11121 - 6565", MailPriority.High, "RefNumber"));
            var command = new SendInternalEmailCommand(Guid.NewGuid(), emailTemplate);
            base.Execute<SendInternalEmailCommand>(command);
            Assert.IsFalse(messages.HasErrors);
        }
    }
}