using ActionMailerNext.Standalone;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Services.Communications.Managers.Email;
using SAHL.Services.Communications.Managers.LiveReplies;
using SAHL.Services.Communications.Specs.Models;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;

namespace SAHL.Services.Communications.Specs.Managers.Mailer
{
    public class when_told_to_generate_email : WithFakes
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
            attachments = new List<IMailAttachment>();

            communicationsMailer = new EmailManager(communicationsManager);

            emailModel = new TestEmailModel("blah@noDomain.xyz", "testing 123", "Lorem ipsum dolor sit amet...", System.Net.Mail.MailPriority.Normal, attachments);
        };

        private Because of = () =>
        {
            result = communicationsMailer.GenerateEmail("TestEmailTemplate", emailModel);
        };

        private It should_return_an_email_reply = () =>
        {
            result.ShouldNotBeNull();
        };
    }
}