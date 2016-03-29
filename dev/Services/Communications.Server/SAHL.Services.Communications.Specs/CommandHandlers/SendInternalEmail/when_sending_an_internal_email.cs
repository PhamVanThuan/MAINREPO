using ActionMailerNext.Standalone;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Communications.CommandHandlers;
using SAHL.Services.Communications.Managers.Email;
using SAHL.Services.Communications.Specs.Fakes;
using SAHL.Services.Communications.Specs.Models;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Communications.Specs.CommandHandlers.SendInternalEmail
{
    public class when_sending_an_internal_email : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static SendInternalEmailCommand command;
        private static SendInternalEmailCommandHandler handler;
        private static IEmailManager communicationsMailer;
        private static RazorEmailResult result;
        private static IEnumerable<IMailAttachment> attachments;

        private Establish context = () =>
        {
            result = null;
            communicationsMailer = An<IEmailManager>();

            messages = new SystemMessageCollection();
            metadata = new ServiceRequestMetadata();
            attachments = new List<IMailAttachment>();


            command = new SendInternalEmailCommand(Guid.NewGuid(), new TestEmailTemplate("TestEmailTemplate",
                new TestEmailModel("blah@noDomain.xyz", "testing123", "Lorem ipsum dolor sit amet...", MailPriority.Normal, attachments)));
            handler = new SendInternalEmailCommandHandler(communicationsMailer);

            communicationsMailer.WhenToldTo(x => x.GenerateEmail(Param.IsAny<string>(), Param.IsAny<IEmailModel>())).Return(result);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };

        private It should_have_the_expected_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldStartWith("An error occurred when generating an email for template: ");
        };
    }
}
