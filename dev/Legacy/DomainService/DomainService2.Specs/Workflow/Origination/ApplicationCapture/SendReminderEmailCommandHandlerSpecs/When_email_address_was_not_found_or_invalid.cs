using DomainService2.Workflow.Origination.ApplicationCapture;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationCapture.SendReminderEmailCommandHandlerSpecs
{
    [Subject(typeof(SendReminderEmailCommandHandler))]
    public class When_email_address_was_not_found_or_invalid : WithFakes
    {
        private static IX2Repository x2Repository;
        private static IMessageService messageService;

        private static SendReminderEmailCommand command;
        private static SendReminderEmailCommandHandler handler;

        private static IDomainMessageCollection messages;

        Establish context = () =>
        {
            x2Repository = An<IX2Repository>();
            messageService = An<IMessageService>();
            messages = An<IDomainMessageCollection>();

            string emailAddress = "";

            x2Repository.WhenToldTo(x => x.GetEmailAddressForCaseOwner(Param.IsAny<long>())).Return(emailAddress);
            messageService.WhenToldTo(x => x.SendEmailInternal(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>())).Return(false);

            command = new SendReminderEmailCommand(Param.IsAny<int>(), Param.IsAny<long>());
            handler = new SendReminderEmailCommandHandler(x2Repository, messageService);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_not_attempt_to_send_email_reminder = () =>
        {
            messageService.WasNotToldTo(x => x.SendEmailInternal(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));
        };
    }
}