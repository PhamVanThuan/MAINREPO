using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Communications.CommandHandlers;
using SAHL.Services.Communications.Managers.Email;
using SAHL.Services.Communications.Managers.LiveReplies;
using SAHL.Services.Communications.Specs.Fakes;
using SAHL.Services.Communications.Specs.Models;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace SAHL.Services.Communications.Specs.CommandHandlers.SendInternalEmail
{
    public class when_told_to_send_an_internal_email_and_no_template_is_found : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static SendInternalEmailCommand command;
        private static SendInternalEmailCommandHandler handler;
        private static IEmailManager communicationsMailer;
        private static ILiveRepliesManager communicationsManager;
        private static IEnumerable<IMailAttachment> attachments;

        private Establish context = () =>
        {
            communicationsManager = An<ILiveRepliesManager>();
            communicationsManager.WhenToldTo(x => x.InternalEmailFromAddress).Return("halo@sahomeloans.com");
            attachments = new List<IMailAttachment>();

            communicationsMailer = new EmailManager(communicationsManager);

            messages = new SystemMessageCollection();
            metadata = new ServiceRequestMetadata();
            command = new SendInternalEmailCommand(Guid.NewGuid(), new TestEmailTemplate("DoesNotExists",
                new TestEmailModel("blah@noDomain.xyz", "testing123", "Lorem ipsum dolor sit amet...", MailPriority.Normal, attachments)));
            handler = new SendInternalEmailCommandHandler(communicationsMailer);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_have_the_expected_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldStartWith("No email template found for: ");
        };
    }
}