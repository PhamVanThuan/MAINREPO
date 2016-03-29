using System;
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
    public class When_sending_internal_mail_for_an_application_that_doesnt_exist : WithFakes
    {
        static SendNTUFinalResubMailCommand command;
        static SendNTUFinalResubMailCommandHandler handler;
        static IDomainMessageCollection messages;
        static Exception exception;

        // Arrange
        Establish context = () =>
            {
                messages = An<IDomainMessageCollection>();
                IApplication application = null;
                IApplicationRepository applicationRepository = An<IApplicationRepository>();
                IMessageService messageService = An<IMessageService>();

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);

                command = new SendNTUFinalResubMailCommand(Param<int>.IsAnything);
                handler = new SendNTUFinalResubMailCommandHandler(applicationRepository, messageService);
            };

        // Act
        Because of = () =>
            {
                exception = Catch.Exception(() => handler.Handle(messages, command));
            };

        // Assert
        It should_throw_an_ArgumentNullException = () =>
            {
                exception.ShouldBeOfType<NullReferenceException>();
            };
    }
}