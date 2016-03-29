using System;
using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.Credit.SendResubMailCommandHandlerSpecs
{
    [Subject(typeof(SendResubMailCommandHandler))]
    public class When_sending_resub_mail : WithFakes
    {
        protected static IDomainMessageCollection messages;
        protected static SendResubMailCommand command;
        protected static SendResubMailCommandHandler handler;
        protected static IApplicationRepository applicationRepository;
        protected static IMessageService messageService;
        protected static int applicationKey = 1;
        protected static IApplication application;
        protected static IAccountSequence reservedAccount;

        // Arrange
        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            messageService = An<IMessageService>();
            application = An<IApplication>();
            reservedAccount = An<IAccountSequence>();

            reservedAccount.WhenToldTo(x => x.Key).Return(1);
            application.WhenToldTo(x => x.ReservedAccount).Return(reservedAccount);
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<Int32>())).Return(application);

            command = new SendResubMailCommand(applicationKey);
            handler = new SendResubMailCommandHandler(applicationRepository, messageService);
        };

        // Act
        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        // Assert
        It should_get_the_application_from_the_repository = () =>
        {
            applicationRepository.WasToldTo(x => x.GetApplicationByKey(applicationKey));
        };

        // Assert
        It should_send_the_mail = () =>
        {
            messageService.WasToldTo(x => x.SendEmailInternal(Param.IsAny<String>(), Param.IsAny<String>(), Param.IsAny<String>(), Param.IsAny<String>(), Param.IsAny<String>(), Param.IsAny<String>()));
        };

        // Assert
        It should_set_command_application_key = () =>
        {
            command.ApplicationKey.Equals(applicationKey);
        };
    }
}