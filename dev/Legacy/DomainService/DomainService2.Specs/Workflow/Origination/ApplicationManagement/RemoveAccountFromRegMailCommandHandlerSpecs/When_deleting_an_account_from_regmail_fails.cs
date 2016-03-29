using System;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.RemoveAccountFromRegMailCommandHandlerSpecs
{
    [Subject(typeof(RemoveAccountFromRegMailCommandHandler))]
    public class When_deleting_an_account_from_regmail_fails : WithFakes
    {
        protected static RemoveAccountFromRegMailCommand command;
        protected static RemoveAccountFromRegMailCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IRegistrationRepository registrationRepository;
        protected static IApplicationRepository applicationRepository;
        protected static Exception exception;

        // Arrange
        Establish context = () =>
        {
            IApplication application = An<IApplication>();
            IAccountSequence accountSequence = An<IAccountSequence>();

            accountSequence.WhenToldTo(x => x.Key).Return(1);

            registrationRepository = An<IRegistrationRepository>();
            registrationRepository.WhenToldTo(x => x.DeleteRegmailByAccountKey(Param<int>.IsAnything)).Throw(new Exception());

            applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

            application.WhenToldTo(x => x.ReservedAccount).Return(accountSequence);

            command = new RemoveAccountFromRegMailCommand(Param<int>.IsAnything);
            handler = new RemoveAccountFromRegMailCommandHandler(registrationRepository, applicationRepository);
        };

        // Act
        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        // Assert
        It should_return_an_exception = () =>
        {
            exception.ShouldNotBeNull();
        };
    }
}