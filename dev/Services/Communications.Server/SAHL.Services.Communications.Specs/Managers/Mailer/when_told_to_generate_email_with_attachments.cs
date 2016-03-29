using ActionMailerNext.Standalone;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using SAHL.Services.Communications.Managers.Email;
using SAHL.Services.Communications.Managers.LiveReplies;
using SAHL.Services.Communications.Specs.Models;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;

namespace SAHL.Services.Communications.Specs.Managers.Mailer
{
    public class when_told_to_generate_email_with_attachments: WithFakes
    {
        private static IEmailManager communicationsMailer;
        private static ILiveRepliesManager communicationsManager;
        private static IEmailModel emailModel;
        private static RazorEmailResult result;
        private static IEnumerable<IMailAttachment> attachments;

        private Establish context = () =>
        {
            communicationsManager = An<ILiveRepliesManager>();
            communicationsManager.WhenToldTo(x => x.InternalEmailFromAddress).Return("halo@sahomeloans.com");
            communicationsMailer = new EmailManager(communicationsManager);
            attachments = new List<IMailAttachment> { new MailAttachment { AttachmentName = "test123.pdf", ContentAsBase64 = "Some Content=", AttachmentType = "pdf" } };

            emailModel = new TestEmailModel("lostcontrol@sahomeloans.com", "Attorney Invoice", "failed to process invoice", System.Net.Mail.MailPriority.High, attachments);

        };

        private Because of = () =>
        {
            result = communicationsMailer.GenerateEmail("TestEmailTemplate", emailModel);
        };

        private It should_contain_two_attachments = () =>
        {
            result.Mail.Attachments.Count.ShouldEqual(1);
        };
    }
}
