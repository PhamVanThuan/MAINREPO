using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.SendNTUFinalResubMailCommandHandlerSpecs
{
    [Subject(typeof(SendNTUFinalResubMailCommandHandler))]
    public class When_sending_internal_mail_for_an_application_that_exists : WithFakes
    {
        protected static SendNTUFinalResubMailCommand command;
        protected static SendNTUFinalResubMailCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static bool sent = true;
        protected static IMessageService messageService;
        protected static int applicationKey = 1;
        protected static int reservedAccountKey = 1;

        // Arrange
        Establish context = () =>
            {
                var applicationRepository = An<IApplicationRepository>();
                messageService = An<IMessageService>();
                IApplication application = An<IApplication>();
                IAccountSequence accountSequence = An<IAccountSequence>();

                application.WhenToldTo(x => x.Key).Return(applicationKey);
                accountSequence.WhenToldTo(x => x.Key).Return(reservedAccountKey);
                application.WhenToldTo(x => x.ReservedAccount).Return(accountSequence);
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsA<int>())).Return(application);

                command = new SendNTUFinalResubMailCommand(Param<int>.IsAnything);
                handler = new SendNTUFinalResubMailCommandHandler(applicationRepository, messageService);

                messageService.WhenToldTo(x => x.SendEmailInternal(Param<string>.IsA<string>(), Param<string>.IsA<string>(), Param<string>.IsA<string>(), Param<string>.IsA<string>(), Param<string>.IsA<string>(), Param<string>.IsA<string>())).Return(sent);
            };

        // Act
        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        // Assert
        It should_send_an_internal_mail = () =>
            {
                messageService.WasToldTo(x => x.SendEmailInternal(Param<string>.IsA<string>(), Param<string>.IsA<string>(), Param<string>.IsA<string>(), Param<string>.IsA<string>(), Param<string>.IsA<string>(), Param<string>.IsA<string>()));
            };
    }
}