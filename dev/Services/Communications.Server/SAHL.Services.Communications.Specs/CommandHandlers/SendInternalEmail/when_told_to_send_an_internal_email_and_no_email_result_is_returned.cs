using ActionMailerNext.Interfaces;
using ActionMailerNext.Standalone;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using RazorEngine.Templating;
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

namespace SAHL.Services.Communications.Specs.CommandHandlers.SendInternalEmail
{
    public class when_told_to_send_an_internal_email_and_no_email_result_is_returned : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static SendInternalEmailCommand command;
        private static SendInternalEmailCommandHandler handler;
        private static IEmailManager emailManager;
        private static RazorEmailResult result;
        private static IEnumerable<IMailAttachment> attachments;

        private Establish context = () =>
        {
            result = new RazorEmailResult(Substitute.For<IMailInterceptor>(), Substitute.For<IMailSender>(), Substitute.For<IMailAttributes>(),
                "viewName", System.Text.Encoding.Unicode, "viewPath", Substitute.For<ITemplateService>(), new DynamicViewBag());
            emailManager = An<IEmailManager>();

            messages = new SystemMessageCollection();
            metadata = new ServiceRequestMetadata();
            attachments = new List<IMailAttachment>();


            command = new SendInternalEmailCommand(Guid.NewGuid(), new TestEmailTemplate("TestEmailTemplate",
                new TestEmailModel("blah@noDomain.xyz", "testing123", "Lorem ipsum dolor sit amet...", MailPriority.Normal, attachments)));
            handler = new SendInternalEmailCommandHandler(emailManager);

            emailManager.WhenToldTo(x => x.GenerateEmail(Param.IsAny<string>(), Param.IsAny<IEmailModel>())).Return(result);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metadata);
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_use_the_email_manager_to_send_the_email = () =>
        {
            emailManager.Received().DeliverEmail(result);
        };
    }
}