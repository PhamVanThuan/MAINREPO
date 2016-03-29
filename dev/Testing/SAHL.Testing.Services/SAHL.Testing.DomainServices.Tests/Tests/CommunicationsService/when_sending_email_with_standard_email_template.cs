using NUnit.Framework;
using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace SAHL.Testing.Services.Tests.CommunicationsService
{
    [TestFixture]
    public class when_sending_email_with_standard_email_template : ServiceTestBase<ICommunicationsServiceClient>
    {
        [Test]
        [Category("RunInLab")]
        public void when_successful()
        {
            List<IMailAttachment> attachments = new List<IMailAttachment> { new MailAttachment { AttachmentName = "test123.pdf", ContentAsBase64 = "Some Content=", AttachmentType = "pdf" } };
            IEmailModel stndModel = new StandardEmailModel("takawirat@sahomeloans.com", "AttorneyInvoice", "testing", MailPriority.High, attachments);
            IEmailTemplate<IEmailModel> emailTemaplate = new StandardEmailTemplate(stndModel);
            var command = new SendInternalEmailCommand(Guid.NewGuid(), emailTemaplate);
            base.Execute<SendInternalEmailCommand>(command);
            Assert.IsFalse(messages.HasErrors);
        }
    }
}